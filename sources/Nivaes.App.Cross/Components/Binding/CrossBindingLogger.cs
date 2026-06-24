using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross;

public static class CrossBindingLogger
{
    public static ILogger? Instance { get; } = CrossLoggerHost.GetLogger("Bind");
}