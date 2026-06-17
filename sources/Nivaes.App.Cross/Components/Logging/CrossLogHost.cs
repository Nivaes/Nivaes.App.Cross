using System.Runtime.CompilerServices;
using System.ServiceModel.Channels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross;

public static class CrossLogHost
{
    static CrossLogHost()
    {
        _defaultLogger = IPlatformApplication.Current!.Services.GetRequiredService<ILoggerFactory>();
    }

    private static ILoggerFactory _defaultLogger;

    public static ILogger Default => GetLogger("Default");

    public static ILogger GetLogger(string categoryName,
        [CallerMemberName] string member = "",
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0) => _defaultLogger.CreateLogger($"{categoryName} - {member} ({file}:{line})");

    public static ILogger GetLogger<T>(
        [CallerMemberName] string member = "",
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0) => _defaultLogger.CreateLogger($"{typeof(T).Name}.{member} ({file}:{line})");
}