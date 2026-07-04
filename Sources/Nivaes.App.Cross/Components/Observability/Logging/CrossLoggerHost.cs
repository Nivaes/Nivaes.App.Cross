using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Observability;

public static class CrossLoggerHost
{
    private static ILoggerFactory _defaultLogger;

    static CrossLoggerHost()
    {
        _defaultLogger = IPlatformApplication.Current!.Services.GetRequiredService<ILoggerFactory>();
    }

    [Obsolete("")]
    public static ILogger Default => GetLogger("Default");

    public static ILogger GetLogger(string categoryName,
        [CallerMemberName] string member = "",
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0) => _defaultLogger.CreateLogger($"{categoryName} - {member} ({file}:{line})");

    public static ILogger<T> GetLogger<T>() => _defaultLogger.CreateLogger<T>();
}