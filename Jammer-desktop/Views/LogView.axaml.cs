using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Jammer_desktop.ViewModels;

namespace Jammer_desktop.Views;

public partial class LogView : UserControl
{
    public LogView()
    {
        InitializeComponent();
    }

    private void BackButton_OnClick(object? sender, RoutedEventArgs e)
    {
        var mainViewModel = (MainViewModel)TopLevel.GetTopLevel(this)!.DataContext!;
        mainViewModel.RouteToSettingsPage();
    }
}