namespace MvvmCross.Binding
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.Logging;

    public static class MvxBindingLog
    {
        public static ILogger? Instance { get; } = MvxLogHost.GetLog("MvxBind");
    }
}