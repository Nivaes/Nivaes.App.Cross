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
                var json = Serialize(ex);
                File.WriteAllText(PathCrashFile, json);
            }
            catch(Exception exx)
            { }
        }

        protected override async Task LoadAndSendException()
        {
            if (File.Exists(PathCrashFile))
            {
                var json = await File.ReadAllTextAsync(PathCrashFile);

                base.Logger.LogCritical(json);
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
