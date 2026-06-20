namespace Nivaes.App.Cross
{
    using System.Reflection;
    using Microsoft.Extensions.Logging;

    public abstract class CrossMainThreadDispatcher :
        ICrossMainThreadDispatcher
    {
        public static void ExceptionMaskedAction(Action action, bool maskExceptions)
        {
            ArgumentNullException.ThrowIfNull(action);

            try
            {
                action();
            }
            catch (TargetInvocationException exception)
            {
                CrossLoggerHost.Default?.LogWarning(exception, "Exception thrown when invoking action via dispatcher");
                if (maskExceptions)
                    CrossLoggerHost.Default?.LogWarning(exception.InnerException, "TargetInvocationException masked");
                else
                    throw;
            }
            catch (Exception exception)
            {
                CrossLoggerHost.Default?.LogWarning(exception, "Exception thrown when invoking action via dispatcher");
                if (maskExceptions)
                    CrossLoggerHost.Default?.LogWarning(exception, "Exception masked");
                else
                    throw;
            }
        }

        public abstract bool RequestMainThreadAction(Action action, bool maskExceptions = true);

        public abstract bool IsOnMainThread { get; }
    }
#nullable restore
}
