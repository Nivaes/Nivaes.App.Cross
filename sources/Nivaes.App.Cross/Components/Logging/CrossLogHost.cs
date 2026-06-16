namespace Nivaes.App.Cross
{
    using System.Reflection.Metadata.Ecma335;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using MvvmCross;
    using Nivaes.IoC;

    [Obsolete]
    public static class CrossLogHost
    {
        static CrossLogHost()
        {
            //var _defaultLogger = Nivaes.Singleton<CrossIoCServiceContainer>.Instance.Resolve<ILoggerFactory>();
            _defaultLogger = IPlatformApplication.Current!.Services.GetRequiredService<ILoggerFactory>();
        }

        private static ILoggerFactory? _defaultLogger;

        public static ILogger? Default => GetLog("Default");

        [Obsolete("", true)]
        public static ILogger<T>? GetLog<T>() => _defaultLogger?.CreateLogger<T>();

        public static ILogger? GetLog(string categoryName) => _defaultLogger?.CreateLogger(categoryName);
    }
}