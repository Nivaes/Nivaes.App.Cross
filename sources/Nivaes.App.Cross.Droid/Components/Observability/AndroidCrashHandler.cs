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

        protected override void SaveException(Exception ex, string description)
        {
            try
            {
                string path = Path.Combine(
                     Application.Context.FilesDir?.AbsolutePath!,
                     "crash.log");

                //var json = File.ReadAllText(file);
                //var json = JsonSerializer.Serialize(ex);

                //var json = File.ReadAllText(file);

                //File.WriteAllText(path, json);

                var logText = $"""
                        {description}
                        {ex.GetType().FullName}
                        message: {ex.Message}
                        stacktrace: {ex.StackTrace}
                        {ex}
                        """;


                base.Logger.LogCritical(logText);
            }
            catch(Exception exx)
            { }
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

            SaveException(ex, "Unhandled Java exception occurred.");

            base.Logger.LogCritical(ex, "Unhandled Java exception occurred.");

            e.Handled = true;
        }
    }
}
