using System.Diagnostics;
using System.ServiceModel.Channels;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using OpenTelemetry.Resources;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Nivaes.App.Cross
{
    public abstract class CrashHandler : ICrashHandler
    {
        protected ILogger Logger { [DebuggerHidden] get; }

        public CrashHandler(ILogger logger) 
        {
            Logger = logger;
        }

        public virtual void Register()
        {
            // Excepciones en código .NET
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            // Excepciones en tareas asíncronas no observadas
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            Task.Run(async () => LoadAndSendException());
        }

        protected abstract void SaveException(Exception ex, string message);

        protected abstract Task LoadAndSendException();

        private void CurrentDomain_UnhandledException(
            object sender,
            UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;

            SaveException(ex, "Unhandled exception occurred.");

            Logger.LogCritical(ex, "Unhandled exception occurred.");
        }

        private void TaskScheduler_UnobservedTaskException(
            object? sender,
            UnobservedTaskExceptionEventArgs e)
        {
            var ex = e.Exception;
            SaveException(ex, "Unobserved task exception occurred.");

            Logger.LogCritical(ex, "Unobserved task exception occurred.");

            e.SetObserved();
        }

        protected static string Serialize(Exception ex)
        {
            var crash = new CrashInfo(ex);
            return Serialize(crash);
        }

        private static string Serialize(CrashInfo crash)
        { 
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Timestamp: {crash.Timestamp}");
            sb.AppendLine($"Type: {crash.Type}");
            sb.AppendLine($"Message: {crash.Message}");
            sb.AppendLine($"StackTrace: {crash.StackTrace}");
            
            if(crash.Data != null && crash.Data.Count > 0)
                sb.AppendLine($"Data: {crash.Data}");

            if (crash.InnerException != null)
            {
                sb.AppendLine("InnerException:");
                sb.AppendLine(Serialize(crash.InnerException));
            }

            return sb.ToString();
        }
    }
}
