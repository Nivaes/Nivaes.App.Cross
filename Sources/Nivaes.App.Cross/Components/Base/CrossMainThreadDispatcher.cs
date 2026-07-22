using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public abstract class CrossMainThreadDispatcher
        : ICrossMainThreadDispatcher
    {
        protected readonly ILogger Logger;

        public CrossMainThreadDispatcher(ILogger logger)
        {
            Logger = logger;
        }

        // ToDo: Refactorizar para que no sea static.
        public void ExceptionMaskedAction(Action action, bool maskExceptions)
        {
            try
            {
                action();
            }
            catch (TargetInvocationException exception)
            {
                if (maskExceptions)
                    Logger.LogError(exception.InnerException, "TargetInvocationException masked");
                else
                    throw;
            }
            catch (Exception exception)
            {
                if (maskExceptions)
                    Logger.LogError(exception, "Exception masked");
                else
                    throw;
            }
        }

        [Obsolete("Use IMainThreadAsyncDispatcher.ExecuteOnMainThreadAsync instead")]
        public abstract bool RequestMainThreadAction(Action action, bool maskExceptions = true);

        public Task ExecuteOnMainThreadAsync(Action action, bool maskExceptions = true)
        {
            return ExecuteOnMainThreadAsync(() =>
            {
                action();
                return Task.CompletedTask;
            }, maskExceptions);
        }

        public async Task ExecuteOnMainThreadAsync(Func<Task> action, bool maskExceptions = true)
        {
            var completion = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
          
            RequestMainThreadAction(async() =>
                {
                    await action();
                    completion.SetResult(true);
                }, maskExceptions);

            if (!completion.Task.IsCompleted)
                await completion.Task.ConfigureAwait(false);
        }

        public abstract bool IsOnMainThread { get; }
    }
}
