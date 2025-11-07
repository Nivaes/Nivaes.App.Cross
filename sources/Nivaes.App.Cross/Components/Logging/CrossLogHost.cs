namespace Nivaes.App.Cross
{
    using Microsoft.Extensions.Logging;

    public static class CrossLogHost
    {
        private static ILogger? _defaultLogger;

        public static ILogger? Default => _defaultLogger ??= GetLog("Default");

        public static ILogger<T>? GetLog<T>() => throw new NotImplementedException();
        //Cross.IoCProvider?.TryResolve<ILoggerFactory>(out var loggerFactory) == true
        //    ? loggerFactory?.CreateLogger<T>()
        //    : null;

        public static ILogger? GetLog(string categoryName) => throw new NotImplementedException();
        //Cross.IoCProvider?.TryResolve<ILoggerFactory>(out var loggerFactory) == true
        //    ? loggerFactory?.CreateLogger(categoryName)
        //    : null;
    }
}