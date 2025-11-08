namespace Nivaes.App.Cross.WinUI3
{
    using Microsoft.UI.Dispatching;

    public sealed class CrossWinUIViewDispatcher 
        : CrossViewDispatcher, ICrossViewDispatcher
    {
        private readonly DispatcherQueue mDispatcher;

        private readonly ICrossViewPresenter mViewPresenter;

        public CrossWinUIViewDispatcher(ICrossViewPresenter viewPresenter, AppDataModel appDataModel) : base()
        {
            mDispatcher = appDataModel.MainFrame.UnderlyingControl.DispatcherQueue;

            mViewPresenter = viewPresenter;
        }

        override public bool IsOnMainThread => mDispatcher.HasThreadAccess;

        public override Task<bool> ShowViewModelOnMainThread(ICrossViewModelRequest request)
        {
            return mViewPresenter.Show(request);
        }

        public override Task<bool> ShowViewModelOnBackgroundThread(ICrossViewModelRequest request, Func<ICrossViewModelRequest, Task<bool>> action)
        {
            var result = mDispatcher.TryEnqueue(DispatcherQueuePriority.Normal, async () =>
            {
                _ = await mViewPresenter.Show(request);
            });

            return Task.FromResult(result);
        }
    }
}
