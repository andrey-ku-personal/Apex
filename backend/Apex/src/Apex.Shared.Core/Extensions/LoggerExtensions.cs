using NLog;

namespace Apex.Shared.Core.Extensions;

public static class LoggerExtensions
{
    public static void LogAppBye(this Logger logger)
    {
        logger.Info("App is finished");
    }
}
