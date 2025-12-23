namespace Nivaes.App.Cross.Color
{
    using Microsoft.Extensions.Logging;

    internal static class MvxPluginLog
    {
        internal static ILogger Instance { get; } = CrossLogHost.GetLog("MvxPlugin.Color");
    }
}
