namespace Nivaes.App.Cross.WinUI
{
    using Microsoft.UI.Dispatching;
    using Nivaes.App.Cross.Presenters;

    public sealed class WinUIViewDispatcher : ViewDispatcher, IViewDispatcher
    {
        private readonly DispatcherQueue _uiDispatcher;

        private readonly IViewPresenter mViewPresenter;

        public WinUIViewDispatcher(IViewPresenter viewPresenter, AppDataModel windowInformation) : base()
        {
            _uiDispatcher = windowInformation.MainFrame.UnderlyingControl.DispatcherQueue;

            mViewPresenter = viewPresenter;
        }

        override public bool IsOnMainThread => _uiDispatcher.HasThreadAccess;

        public override async Task<bool> ShowViewModel(IViewModelRequest request)
        {
            if (_uiDispatcher.HasThreadAccess)
            {
                return await mViewPresenter.Show(request);
            }
            else
            {
                return _uiDispatcher.TryEnqueue(DispatcherQueuePriority.Normal,
                async () =>
                {
                    _ = await mViewPresenter.Show(request);
                });
            }
        }
    }
}
