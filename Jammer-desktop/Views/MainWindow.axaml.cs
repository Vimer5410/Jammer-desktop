using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using Jammer_desktop.ViewModels;
using Serilog;

namespace Jammer_desktop.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        HomeIcon.Foreground = new SolidColorBrush(Color.Parse("#C9444A"));
        Log.Logger = new LoggerConfiguration().WriteTo.Sink(new GuiLogSink()).CreateLogger();
    }
    

    // шапка
    private void TitleDragArea_OnPointerPressed(object? sender, Avalonia.Input.PointerPressedEventArgs e)
    {
        if (e.GetCurrentPoint(this).Properties.IsLeftButtonPressed)
            BeginMoveDrag(e);
    }

    private void MinimizeButton_OnClick(object? sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

    private void CloseButton_OnClick(object? sender, RoutedEventArgs e) => Close();

    private void HomeButton_OnClick(object? sender, RoutedEventArgs e)
    {
        HomeIcon.Foreground = new SolidColorBrush(Color.Parse("#C9444A"));
        SettingsIcon.Foreground = new SolidColorBrush(Color.Parse("#6E747F"));
        
        var mainViewModel = (MainViewModel)DataContext;
        mainViewModel.RouteToHomePage();
    }
    
    
    private void SettingsButton_OnClick(object? sender, RoutedEventArgs e)
    {
        SettingsIcon.Foreground = new SolidColorBrush(Color.Parse("#C9444A"));
        HomeIcon.Foreground = new SolidColorBrush(Color.Parse("#6E747F"));
        
        var mainViewModel = (MainViewModel)DataContext;
        mainViewModel.RouteToSettingsPage();
    }
}