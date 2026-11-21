using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Jammer.Deploy;
using Serilog;

namespace Jammer_desktop.ViewModels;

public partial class SelfHostedViewModel : ViewModelBase
{
    [ObservableProperty]
    private string logText = "Здесь появятся логи установки...";

    [ObservableProperty]
    private string ip = "";

    [ObservableProperty]
    private int port = 22;

    [ObservableProperty]
    private string login = "root";

    [ObservableProperty]
    private string password = "";

    [ObservableProperty] 
    private string installBtnContent = "Установить";
    

    public SelfHostedViewModel()
    {
        GuiLogSink.OnLog += (source, line) =>
        {
            if (source == "SshService" || source=="ServerDelivery")
                LogText += line;
        };
    }
    
    
    
    [RelayCommand]
    private async Task InstallServer()
    {
        InstallBtnContent = "Установка...";
        
       
        LogText = "";
        
        
        try
        {
            await Task.Run(() =>  SshService.DeploySelfHosted(ip, port, login, password));
        }
        catch (Exception ex)
        {
            Log.ForContext("SourceContext", "SshService").Error("{error}", ex.Message);
        }
        finally
        {
            InstallBtnContent = "Установить";
        }
    }
    
}