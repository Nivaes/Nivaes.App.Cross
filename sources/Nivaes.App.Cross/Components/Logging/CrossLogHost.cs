namespace Nivaes.App.Cross
{
    using Microsoft.Extensions.Logging;
    using Nivaes.IoC;

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
    }
}