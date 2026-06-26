using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameHub.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace GameHub.UI.ViewModels;

public partial class LoginViewModel : ObservableObject
{
    private readonly UserRepository _userRepository;
    private readonly MainWindowViewModel _mainViewModel;
    
    [ObservableProperty]
    private string _email = string.Empty;
    
    [ObservableProperty]
    private string _password = string.Empty;
    
    [ObservableProperty]
    private string _errorMessage = string.Empty;


    public LoginViewModel(MainWindowViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        _userRepository = App.ServiceProvider.GetRequiredService<UserRepository>();
    }

    [RelayCommand]
    private async Task LoginAsync()
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Будь ласка, заповніть всі поля.";
            return;
        }

        try
        {
            var user = await _userRepository.GetByEmailAsync(Email);

            if (user == null)
            {
                ErrorMessage = "Користувача з такою електронною поштою не знайдено.";
                return;
            }

            if (user.PasswordHash != Password)
            {
                ErrorMessage = "Невірний пароль";
                return;
            }

            ErrorMessage = $"Успішний вхід! Вітаємо, {user.Role} {user.Username}!";
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Помилка бази даних: {ex.Message}";
        }
    }
    
    
    [RelayCommand]
    private void GoToRegister()
    {
        
    }
}