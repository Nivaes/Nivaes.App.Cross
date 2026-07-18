using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Nivaes.App.Cross.Observability;
using OpenTelemetry.Logs;

namespace Nivaes.App.Cross.WinUI.Observability
{
    internal class WinUICrashHandler : CrashHandler
    {
        protected override string PathCrashFile => Path.Combine(
            Windows.Storage.ApplicationData.Current.LocalFolder.Path,
            "crash.log");

        public WinUICrashHandler(ILogger<WinUICrashHandler> logger, LoggerProvider? loggerProvider = null)
            : base(logger, loggerProvider)
        {
        }

        internal void RegisterApplication(Application app)
        {
            app.UnhandledException += App_UnhandledException;
        }

        private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.Exception;

            SaveException(ex, "Unhandled exception occurred.");

            Logger.LogCritical(ex, "Unhandled exception occurred.");
            LoggerProvider?.ForceFlush();
        }
    }
}
