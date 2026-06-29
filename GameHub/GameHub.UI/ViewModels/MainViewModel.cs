using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace GameHub.UI.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly MainWindowViewModel _mainWindowViewModel;

    [ObservableProperty]
    private object _currentTab = null!;

    [ObservableProperty]
    private string _weatherText = "Завантаження погоди...";

    [ObservableProperty]
    private string _weatherRecommendation = "--------------";

    public MainViewModel(MainWindowViewModel mainWindowViewModel)
    {
        _mainWindowViewModel = mainWindowViewModel;
        GoToCatalog();
        
        // TODO: Сюди ми потім вставимо асинхронний виклик завантаження погоди
    }
    
    [RelayCommand]
    private void GoToCatalog()
    {
        CurrentTab = new CatalogViewModel();
    }

    [RelayCommand]
    private void GoToLibrary()
    {
        // TODO Сюди підставимо ViewModel особистої бібліотеки
    }

    [RelayCommand]
    private void GoToNews()
    {
        // TODO Сюди підставимо Новини ігор
    }

    [RelayCommand]
    private void Logout()
    {
        _mainWindowViewModel.CurrentPage = new LoginViewModel(_mainWindowViewModel);
    }
}