using CommunityToolkit.Mvvm.ComponentModel;

namespace GameHub.UI.ViewModels;

// Головне головне вікно-контейнер (Керує зміною екранів Вхід / Реєстрація / Головна)
public partial class MainWindowViewModel : ViewModelBase
{
    // Сторінка, яка відображається в даний момент в додатку
    [ObservableProperty] private ObservableObject _currentPage;

    public MainWindowViewModel()
    {
        // Стартова сторінка додатку — вікно авторизації
        CurrentPage = new LoginViewModel(this);
    }
}