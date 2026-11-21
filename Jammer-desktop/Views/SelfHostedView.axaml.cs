using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Jammer_desktop.ViewModels;
using Avalonia.VisualTree;
using Jammer.Core;
using Jammer.Deploy;
using Serilog;

namespace Jammer_desktop.Views;

public partial class SelfHostedView : UserControl
{
    public SelfHostedView()
    {
        InitializeComponent();
        
        GuiLogSink.OnLog += (_, _) =>
            Dispatcher.UIThread.Post(() => LogScrollViewer.ScrollToEnd(), DispatcherPriority.Background);
    }
    

    private async void InstallServerButton_OnClick(object? sender, RoutedEventArgs e)
    {
        InstallButton.IsEnabled = false;
        InstallButton.Content = "Установка...";
        
        var selfHostedViewModel = (SelfHostedViewModel)DataContext;
        selfHostedViewModel._logText = "";
        
        string ip = IpTextBox.Text;
        string login = LoginTextBox.Text;
        string password = PasswordTextBox.Text;
        
        int.TryParse(SshPortTextBox.Text, out var port);
        
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
            InstallButton.IsEnabled = true;
            InstallButton.Content = "Установить";
        }
    }

    private void ShowPasswordButton_OnIsCheckedChanged(object? sender, RoutedEventArgs e)
    {
        if (ShowPasswordButton.IsChecked ?? false)
        {
            PasswordTextBox.PasswordChar = '\0';
        }
        else
        {
            PasswordTextBox.PasswordChar = '*';
        }
        
    }
}