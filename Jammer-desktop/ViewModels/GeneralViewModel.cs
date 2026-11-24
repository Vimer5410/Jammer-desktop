using CommunityToolkit.Mvvm.ComponentModel;

namespace Jammer_desktop.ViewModels;

public partial class GeneralViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool startWithWindowsEnable;
    
    
    [ObservableProperty]
    private bool rollTrayEnable;
    
    
    [ObservableProperty]
    private bool writeLogsEnable;
    
    
    [ObservableProperty]
    private int languageSelectedIndex = 0;
}