using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using GameHub.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GameHub.UI.ViewModels;

public partial class CatalogViewModel : ObservableObject
{
    private readonly GameRepository _gameRepository;
    private readonly HttpClient _httpClient;

    [ObservableProperty] private int _currentPage = 1;
    [ObservableProperty] private int _totalCount;
    
    [ObservableProperty] private string _searchText = string.Empty;
    
    [ObservableProperty] private int _selectedSortIndex = 0;

    public ObservableCollection<GenreUiModel> Genres { get; set; } = new();
    
    [ObservableProperty] private GenreUiModel? _selectedGenre;

    public ObservableCollection<GameUiModel> Games { get; set; } = new();

    public CatalogViewModel()
    {
        var context = new Infrastructure.Data.GameHubDbContext();
        _gameRepository = new GameRepository(context); 
    
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 10.0; Win64; x64)");

        _ = LoadGenresAsync();
    }

    private async Task LoadGenresAsync()
    {
        try
        {
            using var context = new Infrastructure.Data.GameHubDbContext();
            var dbGenres = await context.Genres.AsNoTracking().ToListAsync();

            Genres.Clear();
            Genres.Add(new GenreUiModel(0, "Усі жанри"));

            foreach (var g in dbGenres)
            {
                Genres.Add(new GenreUiModel(g.Id, g.Name));
            }

            SelectedGenre = Genres.First();
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Помилка завантаження жанрів: {ex.Message}");
        }
    }

    partial void OnSearchTextChanged(string value)
    {
        CurrentPage = 1;
        _ = LoadPagedGamesAsync();
    }

    partial void OnSelectedSortIndexChanged(int value)
    {
        CurrentPage = 1;
        _ = LoadPagedGamesAsync();
    }

    partial void OnSelectedGenreChanged(GenreUiModel? value)
    {
        CurrentPage = 1;
        _ = LoadPagedGamesAsync();
    }

    public async Task LoadPagedGamesAsync()
    {
        try
        {
            Games.Clear();

            string? sortByParam = SelectedSortIndex switch
            {
                1 => "Title",
                2 => "TitleDesc",
                _ => null
            };

            int? genreIdParam = (SelectedGenre == null || SelectedGenre.Id == 0) ? null : SelectedGenre.Id;

            var (items, totalCount) = await _gameRepository.GetPagedGamesAsync(
                searchItem: string.IsNullOrWhiteSpace(SearchText) ? null : SearchText,
                genreId: genreIdParam,
                sortBy: sortByParam,
                pageNumber: CurrentPage,
                pageSize: 12
            );

            TotalCount = totalCount;

            foreach (var game in items)
            {
                Bitmap? bitmap = null;
                var firstImage = game.Images.FirstOrDefault();

                if (firstImage != null && !string.IsNullOrEmpty(firstImage.ImagePath))
                {
                    try
                    {
                        if (firstImage.ImagePath.StartsWith("http"))
                        {
                            var bytes = await _httpClient.GetByteArrayAsync(firstImage.ImagePath);
                            using var stream = new MemoryStream(bytes);
                            bitmap = new Bitmap(stream);
                        }
                        else if (File.Exists(firstImage.ImagePath))
                        {
                            bitmap = new Bitmap(firstImage.ImagePath);
                        }
                    }
                    catch (Exception imgEx)
                    {
                        System.Diagnostics.Debug.WriteLine($"Помилка картинки: {imgEx.Message}");
                    }
                }

                Games.Add(new GameUiModel(game.Id, game.Title, bitmap));
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Помилка завантаження каталогу: {ex.Message}");
        }
    }

    public async Task NextPageAsync()
    {
        int maxPage = (int)Math.Ceiling((double)TotalCount / 12);
        if (CurrentPage < maxPage)
        {
            CurrentPage++;
            await LoadPagedGamesAsync();
        }
    }

    public async Task PreviousPageAsync()
    {
        if (CurrentPage > 1)
        {
            CurrentPage--;
            await LoadPagedGamesAsync();
        }
    }
}

public class GameUiModel
{
    public int Id { get; set; }
    public string Title { get; set; }
    public Bitmap? Cover { get; set; }

    public GameUiModel(int id, string title, Bitmap? cover)
    {
        Id = id;
        Title = title;
        Cover = cover;
    }
}

public class GenreUiModel
{
    public int Id { get; set; }
    public string Name { get; set; }

    public GenreUiModel(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public override string ToString() => Name;
}