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
            var asyncAction = new Func<Task>(() =>
            {
                action();
                return Task.CompletedTask;
            });
            return ExecuteOnMainThreadAsync(asyncAction, maskExceptions);
        }

        public async Task ExecuteOnMainThreadAsync(Func<Task> action, bool maskExceptions = true)
        {
            var completion = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);
            var syncAction = new Action(async () =>
            {
                await action();
                completion.SetResult(true);
            });
            RequestMainThreadAction(syncAction, maskExceptions);

            // If we're already on main thread, then the action will
            // have already completed at this point, so can just return
            if (completion.Task.IsCompleted)
                return;

            // Make sure we don't introduce weird locking issues  
            // blocking on the completion source by jumping onto
            // a new thread to wait
            await Task.Run(async () => await completion.Task);
        }

        public abstract bool IsOnMainThread { get; }
    }
}
