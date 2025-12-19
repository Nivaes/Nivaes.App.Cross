namespace Nivaes.App.Cross
{
    using System;
    using System.Threading.Tasks;
    //using static System.Net.Mime.MediaTypeNames;

    [Obsolete]
    public abstract class CrossViewDispatcher : ICrossViewDispatcher
    {
        protected CrossViewDispatcher()
        {
        }

        public abstract bool IsOnMainThread { get; }

        public abstract Task<bool> ShowViewModelOnMainThread(ICrossViewModelRequest request);

        public abstract Task<bool> ShowViewModelOnBackgroundThread(ICrossViewModelRequest request, Func<ICrossViewModelRequest, Task<bool>> action);

        public Task<bool> ShowViewModel(ICrossViewModelRequest request)
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

        private async Task<bool> RunBackgroundThread(ICrossViewModelRequest request)
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

        public Task<bool> ChangePresentation(CrossPresentationHint hint)
        {
            throw new NotImplementedException();
        }
    }
}
