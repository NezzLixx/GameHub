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

// Логіка форми створення нового акаунта (Реєстрація)
public partial class RegisterViewModel : ObservableObject
{
    private readonly UserRepository _userRepository;
    private readonly MainWindowViewModel _mainViewModel;
    private readonly EmailService _emailService;
    
    // Поля форми реєстрації
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

    // Команда реєстрації користувача
    [RelayCommand]
    private async Task RegisterAsync()
    {
        ErrorMessage = string.Empty;

        // 1. Валідація: Чи всі поля заповнені
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Username) 
        || string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
        {
            ErrorMessage = "Будь ласка, заповніть всі поля.";
            return;
        }
        
        // 2. Валідація: Перевірка формату Email через регулярний вираз
        string emailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        if (!Regex.IsMatch(Email, emailPattern))
        {
            ErrorMessage = "Некоректний формат електронної пошти.";
            return;
        }

        // 3. Валідація: Збіг паролів
        if (Password != ConfirmPassword)
        {
            ErrorMessage = "Паролі не збігаються.";
            return;
        }

        try
        {
            // 4. Перевірка: Чи вільний Email в БД
            var existingUser = await _userRepository.GetByEmailAsync(Email);
            if (existingUser != null)
            {
                ErrorMessage = "Користувач з такою поштою вже існує.";
                return;
            }
            
            // 5. Створення сутності і збереження в SQLite
            var newUser = new User
            {
                Email = Email,
                Username = Username,
                PasswordHash = GameHub.Infrastructure.Services.PasswordHasher.HashPassword(Password),
                Role = "User"
            };
            await _userRepository.AddAsync(newUser);
            
            // 6. Надсилання привітального листа на Email через MailKit
            await _emailService.SendWelcomeEmailAsync(Email, Username);
            
            ErrorMessage = "Реєстрація успішна! Тепер ви можете увійти.";
            ClearForm();
        }
        catch (Exception ex)
        {
            ErrorMessage = $"Помилка реєстрації: {ex.Message}.";
        }
    }

    // Очищення полів форми
    private void ClearForm()
    {
        Email = string.Empty;
        Username = string.Empty;
        Password = string.Empty;
        ConfirmPassword = string.Empty;
    }

    // Повернення на сторінку входу
    [RelayCommand]
    private void GoToLogin()
    {
        _mainViewModel.CurrentPage = new LoginViewModel(_mainViewModel);
    }
}