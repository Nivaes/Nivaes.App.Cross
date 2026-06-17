using System.Runtime.CompilerServices;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross;

public static class CrossLogHost
{
    static CrossLogHost()
    {
        _defaultLogger = IPlatformApplication.Current!.Services.GetRequiredService<ILoggerFactory>();
    }

    private static ILoggerFactory? _defaultLogger;

    public static ILogger? Default => GetLogger("Default");

    public static ILogger? GetLogger(string categoryName) => _defaultLogger?.CreateLogger(categoryName);

    public static ILogger? GetLogger<T>([CallerMemberName] string member = "") => _defaultLogger?.CreateLogger($"{typeof(T).Name}.{member}");
}