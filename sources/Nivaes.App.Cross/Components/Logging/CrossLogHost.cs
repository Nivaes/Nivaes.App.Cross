namespace MvvmCross.Logging
{
    using Microsoft.Extensions.Logging;

    public static class CrossLogHost
    {
        private static ILogger? _defaultLogger;

        public static ILogger? Default => _defaultLogger ??= GetLog("Default");

        public static ILogger<T>? GetLog<T>() =>
            Mvx.IoCProvider?.TryResolve<ILoggerFactory>(out var loggerFactory) == true
                ? loggerFactory?.CreateLogger<T>()
                : null;

        public static ILogger? GetLog(string categoryName) =>
            Mvx.IoCProvider?.TryResolve<ILoggerFactory>(out var loggerFactory) == true
                ? loggerFactory?.CreateLogger(categoryName)
                : null;
    }
}