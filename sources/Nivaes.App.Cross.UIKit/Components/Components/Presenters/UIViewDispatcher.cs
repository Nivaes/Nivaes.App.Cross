namespace Nivaes.App.Cross.UIKit
{
    using Nivaes.App.Cross.Presenters;

    public sealed class UIViewDispatcher : ViewDispatcher, IViewDispatcher
    {

        private readonly IViewPresenter mViewPresenter;

        public WinUIViewDispatcher(IViewPresenter viewPresenter/*, AppDataModel windowInformation*/) : base()
        {
            //_uiDispatcher = windowInformation.MainFrame.UnderlyingControl.DispatcherQueue;

            mViewPresenter = viewPresenter;
        }

        //override public bool IsOnMainThread => _uiDispatcher.HasThreadAccess;

        public override async Task<bool> ShowViewModel(IViewModelRequest request)
        {
            //if (_uiDispatcher.HasThreadAccess)
            //{
                return await mViewPresenter.Show(request);
            //}
            //else
            //{
            //    return _uiDispatcher.TryEnqueue(DispatcherQueuePriority.Normal,
            //    async () =>
            //    {
            //        _ = await mViewPresenter.Show(request);
            //    });
            //}
        }
    }
}
