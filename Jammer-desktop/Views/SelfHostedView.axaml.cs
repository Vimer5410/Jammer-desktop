using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Jammer_desktop.ViewModels;
using Avalonia.VisualTree;
using Jammer.Core;
using Jammer.Deploy;

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

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        int.TryParse(SshPortTextBox.Text, out var port);
        SshService.DeploySelfHosted(IpTextBox.Text, port, LoginTextBox.Text, PasswordTextBox.Text);
    }
}