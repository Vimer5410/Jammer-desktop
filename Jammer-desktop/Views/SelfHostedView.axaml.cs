using System;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
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
    }

    private void BackButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var settingsView = this.FindAncestorOfType<SettingsView>();
        var settingsViewModel = (SettingsViewModel)settingsView.DataContext;
        
        settingsViewModel.RouteBack();
    }

    private async void Button_OnClick(object? sender, RoutedEventArgs e)
    {
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
    }
}