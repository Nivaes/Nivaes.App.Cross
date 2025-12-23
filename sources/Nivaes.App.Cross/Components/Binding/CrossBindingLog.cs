namespace Nivaes.App.Cross
{
    using Microsoft.Extensions.Logging;

    public static class CrossBindingLog
    {
        public static ILogger? Instance { get; } = CrossLogHost.GetLog("MvxBind");
    }
}