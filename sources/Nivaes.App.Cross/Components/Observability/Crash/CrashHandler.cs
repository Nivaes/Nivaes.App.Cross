using System.Text.Json;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public abstract class CrashHandler : ICrashHandler
    {
        private readonly ILogger _logger;

        protected ILogger Logger => _logger;

        public CrashHandler(ILogger logger) 
        {
            _logger = logger;
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

            _logger.LogCritical(ex, "Unhandled exception occurred.");
        }

        private void TaskScheduler_UnobservedTaskException(
            object? sender,
            UnobservedTaskExceptionEventArgs e)
        {
            var ex = e.Exception;
            SaveException(ex, "Unobserved task exception occurred.");

            _logger.LogCritical(ex, "Unobserved task exception occurred.");

            e.SetObserved();
        }

        protected static string Serialize(Exception ex)
        {
            var crash = new CrashInfo
            {
                Timestamp = DateTime.UtcNow,
                Type = ex.GetType().FullName ?? "",
                Message = ex.Message,
                StackTrace = ex.ToString()
            };

            return JsonSerializer.Serialize(crash);
        }
    }
}
