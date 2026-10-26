using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Jammer_desktop.ViewModels;

public partial class SettingsViewModel : ViewModelBase
{
    private readonly SelfHostedViewModel _selfHosted = new SelfHostedViewModel();
    private readonly GeneralViewModel _general = new GeneralViewModel();
    private readonly LogViewModel _log = new LogViewModel();
    
    [ObservableProperty] private ViewModelBase? _currentPage;
    
    [RelayCommand] public void RouteToSelfHostedPage() => CurrentPage = _selfHosted;

    [RelayCommand] public void RouteToGeneralPage() => CurrentPage = _general;

    [RelayCommand] public void RouteToLogPage() => CurrentPage = _log;
    
}