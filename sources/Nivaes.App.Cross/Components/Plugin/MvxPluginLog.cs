namespace Nivaes.App.Cross.Components.Plugin
{
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    internal static class MvxPluginLog
    {
        internal static ILogger Instance { get; } = CrossLogHost.GetLogger("MvxPlugin.JsonLocalization");
    }
}
