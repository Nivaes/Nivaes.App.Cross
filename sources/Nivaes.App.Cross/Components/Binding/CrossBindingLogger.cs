using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;

namespace Nivaes.App.Cross;

public static class CrossBindingLogger
{
    [Obsolete("")]
    public static ILogger? Instance { get; } = CrossLoggerHost.GetLogger("Bind");

    public static ILogger GetLogger(string categoryName,
        [CallerMemberName] string member = "",
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0) => CrossLoggerHost.GetLogger($"Bind:{categoryName}", member, file, line);

    public static ILogger GetLogger<T>(
        [CallerMemberName] string member = "",
        [CallerFilePath] string file = "",
        [CallerLineNumber] int line = 0) => CrossLoggerHost.GetLogger("$Bing:{typeof(T).Name}", member, file, line);
}