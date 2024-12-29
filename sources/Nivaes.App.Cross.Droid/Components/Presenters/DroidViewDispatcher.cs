namespace Nivaes.App.Cross.Droid
{
    using Nivaes.App.Cross.Presenters;

    public sealed class DroidViewDispatcher : ViewDispatcher, IViewDispatcher
    {
        private readonly IViewPresenter mViewPresenter;

        public DroidViewDispatcher(IViewPresenter viewPresenter/*, WindowInformation windowInformation*/) 
            : base()
        {
            mViewPresenter = viewPresenter;
        }

        public override bool IsOnMainThread => Application.SynchronizationContext == SynchronizationContext.Current;

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
