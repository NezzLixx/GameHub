using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameHub.Infrastructure.Data;
using GameHub.Infrastructure.Entity;
using GameHub.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace GameHub.UI.ViewModels;

public partial class GameDetailsViewModel : ObservableObject
{
    private readonly int _gameId;
    private readonly MainViewModel _mainViewModel;
    private readonly GameRepository _gameRepository;
    private readonly HttpClient _httpClient;
    private string? _tempAttachedFilePath; // Тимчасовий локальний шлях до файлу

    // Дані гри для відображення
    [ObservableProperty] private string _title = string.Empty;
    [ObservableProperty] private string _description = string.Empty;
    [ObservableProperty] private string _developerName = string.Empty;
    [ObservableProperty] private Bitmap? _cover;

    // Дані для форми нового відгуку
    [ObservableProperty] private string _newReviewText = string.Empty;
    [ObservableProperty] private int _selectedRating = 5;
    [ObservableProperty] private string _attachedFileStatus = "Файл не вибрано";
    public ObservableCollection<int> Ratings { get; } = new() { 1, 2, 3, 4, 5 };

    // Список існуючих рецензій користувачів
    public ObservableCollection<ReviewUiModel> Reviews { get; set; } = new();

    public GameDetailsViewModel(int gameId, MainViewModel mainViewModel)
    {
        _gameId = gameId;
        _mainViewModel = mainViewModel;
        _gameRepository = App.ServiceProvider.GetRequiredService<GameRepository>();
        
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

        _ = LoadGameDetailsAsync();
    }

    private async Task LoadGameDetailsAsync()
    {
        try
        {
            var game = await _gameRepository.GetByIdAsync(_gameId);
            if (game == null) return;

            Title = game.Title;
            Description = game.Description;
            DeveloperName = game.Developer?.Name ?? "Невідомий розробник";

            // Завантаження обкладинки гри
            var firstImage = game.Images.FirstOrDefault();
            if (firstImage != null && !string.IsNullOrEmpty(firstImage.ImagePath))
            {
                if (firstImage.ImagePath.StartsWith("http"))
                {
                    var bytes = await _httpClient.GetByteArrayAsync(firstImage.ImagePath);
                    using var stream = new MemoryStream(bytes);
                    Cover = new Bitmap(stream);
                }
                else if (File.Exists(firstImage.ImagePath))
                {
                    Cover = new Bitmap(firstImage.ImagePath);
                }
            }

            // Завантаження відгуків та їх конвертація
            Reviews.Clear();
            foreach (var r in game.Reviews.OrderByDescending(x => x.CreatedAt))
            {
                Reviews.Add(new ReviewUiModel(
                    r.User?.Username ?? "Гість",
                    r.Rating,
                    r.ReviewText,
                    r.AttachedImageData // Передаємо масив байтів прямо з бази даних
                ));
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Помилка деталей гри: {ex.Message}");
        }
    }

    public void AttachFile(string localPath)
    {
        if (File.Exists(localPath))
        {
            _tempAttachedFilePath = localPath;
            AttachedFileStatus = $"📎 {Path.GetFileName(localPath)} (Готовий до завантаження)";
        }
    }

    // Команда надсилання рецензії в БД
    [RelayCommand]
    private async Task SubmitReviewAsync()
    {
        if (string.IsNullOrWhiteSpace(NewReviewText)) return;

        try
        {
            byte[]? fileBytes = null;

            // Якщо файл був вибраний — асинхронно зчитуємо його в масив байтів
            if (!string.IsNullOrEmpty(_tempAttachedFilePath) && File.Exists(_tempAttachedFilePath))
            {
                fileBytes = await File.ReadAllBytesAsync(_tempAttachedFilePath);
            }

            using var context = new GameHubDbContext();

            // Тимчасова перевірка на Поточного Юзера. Якщо в додатку ще немає глобального App.CurrentUser,
            // беремо першого ліпшого з бази, щоб не падала помилка. Заміни на свій App.CurrentUser.Id!
            var userId = context.Users.FirstOrDefault()?.Id ?? 1;

            var newReview = new Review
            {
                GameId = _gameId,
                UserId = userId, 
                ReviewText = NewReviewText,
                Rating = SelectedRating,
                CreatedAt = DateTime.UtcNow,
                AttachedImageData = fileBytes 
            };

            context.Reviews.Add(newReview);
            await context.SaveChangesAsync();

            // Очищення форми
            NewReviewText = string.Empty;
            _tempAttachedFilePath = null;
            AttachedFileStatus = "Файл не вибрано";

            // Оновлюємо список відгуків на екрані
            await LoadGameDetailsAsync();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Помилка збереження рецензії: {ex.Message}");
        }
    }

    // Команда повернення назад до каталогу ігор
    [RelayCommand]
    private void GoBack()
    {
        var catalog = new CatalogViewModel();
        catalog.Initialize(_mainViewModel);
        _mainViewModel.CurrentTab = catalog;
    }

    [RelayCommand]
    private void GoToDeveloper()
    {
        // переход на сторінку розробника 
    }
}

// UI Модель представлення одного відгуку
public class ReviewUiModel
{
    public string Username { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public Bitmap? AttachedImage { get; set; } // Конвертована картинка

    public ReviewUiModel(string username, int rating, string comment, byte[]? imageData)
    {
        Username = username;
        Rating = rating;
        Comment = comment;

        // Конвертація BLOB (byte[]) з бази даних назад в об'єкт зображення Bitmap
        if (imageData != null && imageData.Length > 0)
        {
            try
            {
                using var stream = new MemoryStream(imageData);
                AttachedImage = new Bitmap(stream);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Помилка конвертації байтів картинки: {ex.Message}");
            }
        }
    }
}