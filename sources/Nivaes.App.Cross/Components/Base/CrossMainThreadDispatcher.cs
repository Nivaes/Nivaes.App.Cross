using System.Reflection;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;

namespace Nivaes.App.Cross
{
    public abstract class CrossMainThreadDispatcher :
        ICrossMainThreadDispatcher
    {
        protected readonly ILogger Logger;

        public CrossMainThreadDispatcher(ILogger logger)
        {
            Logger = logger;
        }

        // ToDo: Refactorizar para que no sea static.
        public static void ExceptionMaskedAction(Action action, bool maskExceptions)
        {
            ArgumentNullException.ThrowIfNull(action);

            ILogger logger = CrossLoggerHost.GetLogger<CrossMainThreadDispatcher>();

            try
            {
                action();
            }
            catch (TargetInvocationException exception)
            {
                logger.LogWarning(exception, "Exception thrown when invoking action via dispatcher");
                if (maskExceptions)
                    logger.LogWarning(exception.InnerException, "TargetInvocationException masked");
                else
                    throw;
            }
            catch (Exception exception)
            {
                logger.LogWarning(exception, "Exception thrown when invoking action via dispatcher");
                if (maskExceptions)
                    logger.LogWarning(exception, "Exception masked");
                else
                    throw;
            }
        }

        public abstract bool RequestMainThreadAction(Action action, bool maskExceptions = true);

        public abstract bool IsOnMainThread { get; }
    }
}
