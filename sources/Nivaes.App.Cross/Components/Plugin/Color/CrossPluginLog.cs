namespace Nivaes.App.Cross.Color
{
    using Microsoft.Extensions.Logging;

    internal static class CrossPluginLog
    {
        internal static ILogger Instance { get; } = CrossLogHost.GetLog("MvxPlugin.Color");
    }
}
