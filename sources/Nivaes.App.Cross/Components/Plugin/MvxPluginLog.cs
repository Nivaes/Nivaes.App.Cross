namespace Nivaes.App.Cross.Components.Plugin
{
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Observability;

    [Obsolete("", true)]
    internal static class MvxPluginLog
    {
        internal static ILogger Instance { get; } = CrossLoggerHost.GetLogger("MvxPlugin.JsonLocalization");
    }
}
