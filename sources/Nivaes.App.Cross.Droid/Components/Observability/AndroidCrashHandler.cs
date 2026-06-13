using System.Text.Json;
using Android.Runtime;
using Microsoft.Extensions.Logging;
using System.IO;

namespace Nivaes.App.Cross.Droid
{
    public class AndroidCrashHandler : CrashHandler
    {
        public AndroidCrashHandler(ILogger<AndroidCrashHandler> logger)
            : base(logger)
        {
        }

        public override void Register()
        {
            base.Register();
            // Excepciones en código Java
            AndroidEnvironment.UnhandledExceptionRaiser += AndroidEnvironment_UnhandledExceptionRaiser;
        }

        protected override void SaveException(Exception ex)
        {
            string path = Path.Combine(
                 Application.Context.FilesDir?.AbsolutePath!,
                 "crash.log");

            //var json = File.ReadAllText(file);
            var json = JsonSerializer.Serialize(ex);

            //var json = File.ReadAllText(file);

            File.WriteAllText(path, json);
        }

        protected override void LoadAndSendException(Exception ex)
        {
            string path = Path.Combine(
                 Application.Context.FilesDir?.AbsolutePath!,
                 "crash.log");

            var json = File.ReadAllText(path);
        }

        private void AndroidEnvironment_UnhandledExceptionRaiser(
            object? sender,
            RaiseThrowableEventArgs e)
        {
            var ex = e.Exception;

            SaveException(ex);

            base.Logger.LogCritical(ex, "Unhandled Java exception occurred.");

            // Indica que la excepción ha sido manejada
            e.Handled = true;
        }
    }
}
