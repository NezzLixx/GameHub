using CommunityToolkit.Mvvm.ComponentModel;

namespace GameHub.UI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty]
    private ObservableObject _currentPage;

    public MainWindowViewModel()
    {
        CurrentPage = new LoginViewModel(this);
    }
}