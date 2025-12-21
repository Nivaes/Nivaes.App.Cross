namespace Nivaes.App.Cross
{
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Base;
    using MvvmCross.Logging;

    public abstract class CrossMainThreadDispatcher : CrossSingleton<ICrossMainThreadDispatcher>, ICrossMainThreadDispatcher
    {
        public static void ExceptionMaskedAction(Action action, bool maskExceptions)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            try
            {
                action();
            }
            catch (TargetInvocationException exception)
            {
                MvxLogHost.Default?.LogWarning(exception, "Exception thrown when invoking action via dispatcher");
                if (maskExceptions)
                    MvxLogHost.Default?.LogWarning(exception.InnerException, "TargetInvocationException masked");
                else
                    throw;
            }
            catch (Exception exception)
            {
                MvxLogHost.Default?.LogWarning(exception, "Exception thrown when invoking action via dispatcher");
                if (maskExceptions)
                    MvxLogHost.Default?.LogWarning(exception, "Exception masked");
                else
                    throw;
            }
        }

        public abstract bool RequestMainThreadAction(Action action, bool maskExceptions = true);

        public abstract bool IsOnMainThread { get; }
    }
#nullable restore
}
