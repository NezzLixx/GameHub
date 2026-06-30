using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace GameHub.UI.ViewModels;

// Логіка головної робочої зони додатку (після авторизації)
public partial class MainViewModel : ObservableObject
{
    private readonly MainWindowViewModel _mainWindowViewModel;

    // Поточна відкрита вкладка (Каталог, Бібліотека тощо)
    [ObservableProperty] private object _currentTab = null!;

    // Тексти для майбутнього блоку віджета погоди
    [ObservableProperty] private string _weatherText = "Завантаження погоди...";
    [ObservableProperty] private string _weatherRecommendation = "--------------";

    public MainViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
        GoToCatalog(); // При старті відкриваємо каталог за замовчуванням
    }
    
    // Перемикання вкладки на Каталог ігор
    [RelayCommand]
    private void GoToCatalog()
    {
        var catalog = new CatalogViewModel();
        catalog.Initialize(this);
        CurrentTab = catalog;
    }

    // Перемикання вкладки на Особисту бібліотеку
    [RelayCommand]
    private void GoToLibrary()
    {
        // TODO: ViewModel особистої бібліотеки
    }

    // Перемикання вкладки на Новини
    [RelayCommand]
    private void GoToNews()
    {
        // TODO: ViewModel Новин ігор
    }

    // Кнопка виходу з акаунту (повертає на вікно Login)
    [RelayCommand]
    private void Logout()
    {
        _mainWindowViewModel.CurrentPage = new LoginViewModel(_mainWindowViewModel);
    }
}