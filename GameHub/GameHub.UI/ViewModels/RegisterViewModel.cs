using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GameHub.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System.Text.RegularExpressions; 
using GameHub.Infrastructure.Entity;
using GameHub.Infrastructure.Services;

namespace GameHub.UI.ViewModels;

public partial class RegisterViewModel : ObservableObject
{
    private readonly UserRepository _userRepository;
    private readonly MainWindowViewModel _mainViewModel;
    private readonly EmailService _emailService;
    
    [ObservableProperty] private string _email = string.Empty; 
    [ObservableProperty] private string _username = string.Empty;
    [ObservableProperty] private string _password = string.Empty;
    [ObservableProperty] private string _confirmPassword = string.Empty;
    [ObservableProperty] private string _errorMessage = string.Empty;

    public RegisterViewModel(MainWindowViewModel mainViewModel)
    {
        _mainViewModel = mainViewModel;
        _userRepository = App.ServiceProvider.GetRequiredService<UserRepository>();
        _emailService = App.ServiceProvider.GetRequiredService<EmailService>();
    }

    [RelayCommand]
    private async Task RegisterAsync()
    {
        ErrorMessage = string.Empty;

        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Username) 
        || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            ErrorMessage = "Будь ласка, заповніть всі поля.";
            return;
        }
        
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(Email, emailPattern))
        {
            ErrorMessage = "Некоректний формат електронної пошти.";
            return;
        }

        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Паролі не збігаються.";
            return;
        }

        try
        {
            var existingUser = await _userRepository.GetByEmailAsync(Email);
            if (existingUser != null)
            {
                ErrorMessage = "Користувач з такою поштою вже існує.";
                return;
            }
            
            var newUser = new User
            {
                Email = Email,
                Username = Username,
                PasswordHash = Password,
                Role = "User"
            };
            
            await _userRepository.AddAsync(newUser);
            
            await _emailService.SendWelcomeEmailAsync(Email, Username);
            
            ErrorMessage = "Реєстрація успішна! Тепер ви можете увійти.";
            
            Email = string.Empty;
            Username = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Помилка реєстрації: {ex.Message}.";
        }
    }

    [RelayCommand]
    private void GoToLogin()
    {
        _mainViewModel.CurrentPage = new LoginViewModel(_mainViewModel);
    }
}