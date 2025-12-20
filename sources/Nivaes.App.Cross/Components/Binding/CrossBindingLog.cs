namespace Nivaes.App.Cross
{
    using Microsoft.Extensions.Logging;

    [Obsolete("Quitar IoC de Cross")]
    public static class CrossBindingLog
    {
        public static ILogger? Instance { get; } = CrossLogHost.GetLog("CrossBind");
    }
}