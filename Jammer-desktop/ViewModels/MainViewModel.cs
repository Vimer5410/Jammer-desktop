using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Jammer_desktop.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly HomeViewModel _home = new HomeViewModel();
    private readonly SettingsViewModel _settings = new SettingsViewModel();
    
    [ObservableProperty] private ViewModelBase _currentPage;

    public MainViewModel() => _currentPage = _home;

    [RelayCommand] public void RouteToHomePage() => CurrentPage = _home;
    
    [RelayCommand] public void RouteToSettingsPage() => CurrentPage = _settings;

}