namespace MvvmCross.Binding
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.Logging;

    public static class CrossBindingLog
    {
        public static ILogger? Instance { get; } = MvxLogHost.GetLog("MvxBind");
    }
}