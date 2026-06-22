using Android.Runtime;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Logs;
using OpenTelemetry.Trace;

namespace Nivaes.App.Cross.Droid
{
    public class AndroidCrashHandler : CrashHandler
    {
        private string PathCrashFile => Path.Combine(
                     Application.Context.FilesDir?.AbsolutePath!,
                     "crash.log");

        public AndroidCrashHandler(ILogger<AndroidCrashHandler> logger, LoggerProvider loggerFactory)
            : base(logger, loggerFactory)
        {
        }

        public override void Register()
        {
            base.Register();
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
        }

        protected override void SaveException(Exception ex, string description)
        {
            try
            {
                var message = Serialize(ex);
                File.WriteAllText(PathCrashFile, message);
            }
            catch { }
        }

        protected override async Task LoadAndSendException()
        {
            if (File.Exists(PathCrashFile))
            {
                var message = await File.ReadAllTextAsync(PathCrashFile);

                base.Logger.LogCritical(message);
                LoggerProvider.ForceFlush();

            }
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(
            object? sender,
            RaiseThrowableEventArgs e)
        {
            var ex = e.Exception;

            SaveException(ex, "Unhandled Java exception occurred.");

            base.Logger.LogCritical(ex, "Unhandled Java exception occurred.");
            LoggerProvider.ForceFlush();

            e.Handled = true;
        }
    }
}
