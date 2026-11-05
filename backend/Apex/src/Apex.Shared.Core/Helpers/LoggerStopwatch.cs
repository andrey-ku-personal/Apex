using System.Diagnostics;

namespace Apex.Shared.Core.Helpers;

public class LoggerStopwatch : IDisposable, IAsyncDisposable
{
    private readonly Stopwatch _sw;
    private readonly Action<TimeSpan> _logAction;

    public LoggerStopwatch(Action<TimeSpan> logAction)
    {
        _logAction = logAction;
        _sw = Stopwatch.StartNew();
    }

    public void Dispose()
    {
        _sw.Stop();
        _logAction(_sw.Elapsed);
    }

    public async ValueTask DisposeAsync()
    {
        await Task.Run(Dispose);
        GC.SuppressFinalize(this);
    }
}