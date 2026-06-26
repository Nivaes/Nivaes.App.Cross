namespace Nivaes.App.Cross.Droid
{
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross.Observability;

    [Obsolete("", true)]
    internal static class MvxAndroidLog
    {
        internal static ILogger Instance { get; } = CrossLoggerHost.GetLogger("Leanback");
    }
}
