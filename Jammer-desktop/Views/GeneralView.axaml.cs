using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Jammer_desktop.ViewModels;
using Avalonia.VisualTree;

namespace Jammer_desktop.Views;

public partial class GeneralView : UserControl
{
    public GeneralView()
    {
        InitializeComponent();
    }

    private void BackButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var settingsView = this.FindAncestorOfType<SettingsView>();
        var settingsViewModel = (SettingsViewModel)settingsView.DataContext;
        
        settingsViewModel.RouteBack();
    }
}