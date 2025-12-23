namespace MvvmCross.Plugin.JsonLocalization
{
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    internal static class MvxPluginLog
    {
        internal static ILogger Instance { get; } = CrossLogHost.GetLog("MvxPlugin.JsonLocalization");
    }
}
