using Android.Runtime;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;

namespace Nivaes.App.Cross.Droid.Observability
{
    public class AndroidCrashHandler : CrashHandler
    {
        protected override string PathCrashFile => Path.Combine(
                     Application.Context.FilesDir?.AbsolutePath!,
                     "crash.log");

        public AndroidCrashHandler(ILogger<AndroidCrashHandler> logger, LoggerProvider? loggerProvider = null)
            : base(logger, loggerProvider)
        {
        }

        public override void Register()
        {
            base.Register();
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(
            object? sender,
            RaiseThrowableEventArgs e)
        {
            var ex = e.Exception;

            SaveException(ex, "Unhandled Java exception occurred.");

            base.Logger.LogCritical(ex, "Unhandled Java exception occurred.");
            LoggerProvider?.ForceFlush();

            e.Handled = true;
        }
    }
}
