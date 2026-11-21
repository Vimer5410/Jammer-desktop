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

namespace Jammer_desktop.Views;

public partial class SelfHostedView : UserControl
{
    public SelfHostedView()
    {
        InitializeComponent();
    }

    protected override void OnAttachedToVisualTree(VisualTreeAttachmentEventArgs e)
    {
        base.OnAttachedToVisualTree(e);
        GuiLogSink.OnLog += ScrollLogToEnd;
    }

    protected override void OnDetachedFromVisualTree(VisualTreeAttachmentEventArgs e)
    {
        GuiLogSink.OnLog -= ScrollLogToEnd;
        base.OnDetachedFromVisualTree(e);
    }

    private void ScrollLogToEnd(string source, string line)
    {
        Dispatcher.UIThread.Post(() => LogScrollViewer.ScrollToEnd(), DispatcherPriority.Background);
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