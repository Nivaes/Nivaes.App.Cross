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
        }

        private void CurrentDomain_UnhandledException(
            object sender,
            UnhandledExceptionEventArgs e)
        {
            var ex = (Exception)e.ExceptionObject;

            _logger.LogCritical(ex, "Unhandled exception occurred.");
        }

        private void TaskScheduler_UnobservedTaskException(
            object? sender,
            UnobservedTaskExceptionEventArgs e)
        {
            _logger.LogCritical(e.Exception, "Unobserved task exception occurred.");

            e.SetObserved();
        }
    }
}
