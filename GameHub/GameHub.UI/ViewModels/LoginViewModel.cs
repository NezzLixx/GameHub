using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameHub.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace GameHub.UI.ViewModels;

// Логіка сторінки авторизації (входу)
public partial class LoginViewModel : ObservableObject
{
    private readonly UserRepository _userRepository;
    private readonly MainWindowViewModel _mainViewModel;
    
    // Дані форми входу
    [ObservableProperty] private string _email = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private string _errorMessage = string.Empty;

    public LoginViewModel(MainWindowViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        _userRepository = App.ServiceProvider.GetRequiredService<UserRepository>(); // DI контейнер
    }

    // Команда обробки кліку на кнопку "Увійти"
    [RelayCommand]
    private async Task LoginAsync()
    {
        ErrorMessage = string.Empty;

        // Валідація заповнення полів
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Будь ласка, заповніть всі поля.";
            return;
        }

        try
        {
            var user = await _userRepository.GetByEmailAsync(Email);

            // Перевірка існування користувача
            if (user == null)
            {
                ErrorMessage = "Користувача з такою електронною поштою не знайдено.";
                return;
            }

            // Перевірка відповідності пароля
            var hashedInputPassword = GameHub.Infrastructure.Services.PasswordHasher.HashPassword(Password);

            if (user.PasswordHash != hashedInputPassword)
            {
                ErrorMessage = "Невірний пароль";
                return;
            }

            // Успішна авторизація -> перенаправлення на головний екран додатку
            ErrorMessage = $"Успішний вхід! Вітаємо, {user.Role} {user.Username}!";
            _mainViewModel.CurrentPage = new MainViewModel(_mainViewModel);
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Помилка бази даних: {ex.Message}";
        }
    }
    
    // Перехід на форму реєстрації
    [RelayCommand]
    private void GoToRegister()
    {
        _mainViewModel.CurrentPage = new RegisterViewModel(_mainViewModel);
    }
}