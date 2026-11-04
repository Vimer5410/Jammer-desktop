using System;
using Avalonia.Threading;
using Serilog.Core;
using Serilog.Events;

namespace Jammer_desktop;

public class GuiLogSink : ILogEventSink
{
    public static event Action<string, string>? OnLog;

    public void Emit(LogEvent e)
    {
        string source = "";
        if (e.Properties.TryGetValue("SourceContext", out var value) && value is ScalarValue scalar)
            source = scalar.Value?.ToString() ?? "";

        string line = e.RenderMessage() + "\n";
        Dispatcher.UIThread.Post(() => OnLog?.Invoke(source, line));
    }
}