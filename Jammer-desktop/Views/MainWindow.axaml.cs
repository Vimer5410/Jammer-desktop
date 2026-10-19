using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Jammer_desktop.ViewModels;

namespace Jammer_desktop.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        HomeIcon.Foreground = new SolidColorBrush(Color.Parse("#C9444A"));
    }
    

    private void HomeButton_OnClick(object? sender, RoutedEventArgs e)
    {
        HomeIcon.Foreground = new SolidColorBrush(Color.Parse("#C9444A"));
        SettingsIcon.Foreground = new SolidColorBrush(Color.Parse("#6B6F80"));
        
        var mainViewModel = (MainViewModel)DataContext;
        mainViewModel.RouteToHomePage();
    }
    
    
    private void SettingsButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SettingsIcon.Foreground = new SolidColorBrush(Color.Parse("#C9444A"));
        HomeIcon.Foreground = new SolidColorBrush(Color.Parse("#6B6F80"));
        
        var mainViewModel = (MainViewModel)DataContext;
        mainViewModel.RouteToSettingsPage();
    }
}