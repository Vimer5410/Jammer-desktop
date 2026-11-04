using CommunityToolkit.Mvvm.ComponentModel;

namespace Jammer_desktop.ViewModels;

public partial class SelfHostedViewModel : ViewModelBase
{
    [ObservableProperty]
    public string _logText = "Здесь появятся логи установки...";

    public SelfHostedViewModel()
    {
        GuiLogSink.OnLog += (source, line) =>
        {
            if (source == "SshService")
                LogText += line;
        };
    }
}