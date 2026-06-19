using Android.Runtime;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Droid
{
    public class AndroidCrashHandler : CrashHandler
    {
        private string PathCrashFile => Path.Combine(
                     Application.Context.FilesDir?.AbsolutePath!,
                     "crash.log");

        public AndroidCrashHandler(ILogger<AndroidCrashHandler> logger)
            : base(logger)
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
            catch(Exception exx)
            { }
        }

        protected override async Task LoadAndSendException()
        {
            if (File.Exists(PathCrashFile))
            {
                var message = await File.ReadAllTextAsync(PathCrashFile);

                base.Logger.LogCritical(message);
                base.Logger.LogDebug(message);
                base.Logger.LogTrace("LoadAndSendException");
                base.Logger.LogCritical("LoadAndSendException");
                base.Logger.LogError("LoadAndSendException");

                File.Delete(PathCrashFile);
            }
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(
            object? sender,
            RaiseThrowableEventArgs e)
        {
            var ex = e.Exception;

            SaveException(ex, "Unhandled Java exception occurred.");

            base.Logger.LogCritical(ex, "Unhandled Java exception occurred.");

            e.Handled = true;
        }
    }
}
