using System.Diagnostics;
using System.Net;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Jammer_desktop.ViewModels;

namespace Jammer_desktop.Views;

public partial class SettingsView : UserControl
{
    public SettingsView()
    {
        InitializeComponent();
    }

    private void SelfHostedButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var settingsViewModel = (SettingsViewModel)DataContext;
        settingsViewModel.RouteToSelfHostedPage();
    }

    private void GeneralButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var settingsViewModel = (SettingsViewModel)DataContext;
        settingsViewModel.RouteToGeneralPage();
    }

    private void LogButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var settingsViewModel = (SettingsViewModel)DataContext;
        settingsViewModel.RouteToLogPage();
    }

    private void AboutButton_OnClick(object? sender, RoutedEventArgs e)
    {
        Process.Start(new ProcessStartInfo("https://github.com/Vimer5410/Jammer") { UseShellExecute = true });
    }
}