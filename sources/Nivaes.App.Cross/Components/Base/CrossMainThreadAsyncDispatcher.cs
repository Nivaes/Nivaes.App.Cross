namespace Nivaes.App.Cross
{
    using System;
    using System.Threading.Tasks;

    public abstract class CrossMainThreadAsyncDispatcher
        : CrossMainThreadDispatcher, ICrossMainThreadAsyncDispatcher
    {
        public Task ExecuteOnMainThreadAsync(Action action, bool maskExceptions = true)
        {
            if (action == null)
                return Task.CompletedTask;

            var asyncAction = new Func<Task>(() =>
            {
                action();
                return Task.CompletedTask;
            });
            return ExecuteOnMainThreadAsync(asyncAction, maskExceptions);
        }

        public async Task ExecuteOnMainThreadAsync(Func<Task> action, bool maskExceptions = true)
        {
            if (action == null)
                return;

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

        public abstract override bool IsOnMainThread { get; }
    }
}
