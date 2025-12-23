namespace Nivaes.App.Cross.Droid
{
    using Microsoft.Extensions.Logging;

    internal static class MvxAndroidLog
    {
        internal static ILogger Instance { get; } = CrossLogHost.GetLog("Leanback");
    }
}
