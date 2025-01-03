namespace Nivaes.App.Cross
{
    using System.Threading.Tasks;
    using static System.Net.Mime.MediaTypeNames;

    public abstract class ViewDispatcher : IViewDispatcher
    {
        protected ViewDispatcher()
        {
        }

        public abstract bool IsOnMainThread { get; }

        public abstract Task<bool> ShowViewModelOnMainThread(IViewModelRequest request);

        public abstract Task<bool> ShowViewModelOnBackgroundThread(IViewModelRequest request, Func<IViewModelRequest, Task<bool>> action);

        public Task<bool> ShowViewModel(IViewModelRequest request)
        {
            if (IsOnMainThread)
            {
                return ShowViewModelOnMainThread(request);
            }
            else
            {
                return RunBackgroundThread(request);
            }
        }

        private async Task<bool> RunBackgroundThread(IViewModelRequest request)
        {
            var completion = new TaskCompletionSource<bool>(TaskCreationOptions.RunContinuationsAsynchronously);

            var result = await ShowViewModelOnBackgroundThread(request, async (request) =>
            {
                var result = await ShowViewModelOnMainThread(request);
                completion.TrySetResult(result);
                return result;
            });

            if (!completion.Task.IsCompleted)
            {
                await Task.Run(async () =>
                {
                    await completion.Task;
                });
            }

            return await completion.Task;
        }
    }
}
