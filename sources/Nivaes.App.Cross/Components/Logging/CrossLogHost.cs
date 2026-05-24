namespace Nivaes.App.Cross
{
    using Microsoft.Extensions.Logging;
    using MvvmCross;
    using Nivaes.IoC;

    [Obsolete]
    public static class CrossLogHost
    {
        static CrossLogHost()
        {
            var _defaultLogger = Nivaes.Singleton<CrossIoCServiceContainer>.Instance.Resolve<ILoggerFactory>();
        }

        private static ILoggerFactory? _defaultLogger;

        public static ILogger? Default => GetLog("Default");

        public static ILogger<T>? GetLog<T>() => _defaultLogger?.CreateLogger<T>();

        public static ILogger? GetLog(string categoryName) => _defaultLogger?.CreateLogger(categoryName);

        //private static ILogger? _defaultLogger;

        //public static ILogger? Default => _defaultLogger ??= GetLog("Default");

        //public static ILogger<T>? GetLog<T>() =>
        //    Mvx.IoCProvider?.TryResolve<ILoggerFactory>(out var loggerFactory) == true
        //        ? loggerFactory?.CreateLogger<T>()
        //        : null;

        //public static ILogger? GetLog(string categoryName) =>
        //    Mvx.IoCProvider?.TryResolve<ILoggerFactory>(out var loggerFactory) == true
        //        ? loggerFactory?.CreateLogger(categoryName)
        //        : null;
    }
}