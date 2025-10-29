namespace Nivaes.App.Cross.WinUI
{
    using Microsoft.UI.Dispatching;
    using Nivaes.App.Cross.Presenters;

    public sealed class WinUIViewDispatcher 
        : ViewDispatcher, IViewDispatcher
    {
        private readonly DispatcherQueue mDispatcher;

        private readonly IViewPresenter mViewPresenter;

        public WinUIViewDispatcher(IViewPresenter viewPresenter, AppDataModel appDataModel) : base()
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
