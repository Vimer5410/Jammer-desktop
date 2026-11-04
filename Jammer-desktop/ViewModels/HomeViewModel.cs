using CommunityToolkit.Mvvm.ComponentModel;

namespace Jammer_desktop.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    [ObservableProperty]
    public string _logText = "Консольный вывод...";

    public HomeViewModel()
    {
        GuiLogSink.OnLog += (source, line) =>
        {
            if (source != "SshService")
                LogText += line;
        };
    }
}