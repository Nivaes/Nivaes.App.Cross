namespace Nivaes.App.Cross.Droid
{
    using Microsoft.Extensions.Logging;

    [Obsolete("", true)]
    internal static class MvxAndroidLog
    {
        internal static ILogger Instance { get; } = CrossLogHost.GetLog("Leanback");
    }
}
