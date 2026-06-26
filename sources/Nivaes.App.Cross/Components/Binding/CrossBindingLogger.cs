using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;

namespace Nivaes.App.Cross;

public static class CrossBindingLogger
{
    public static ILogger? Instance { get; } = CrossLoggerHost.GetLogger("Bind");
}