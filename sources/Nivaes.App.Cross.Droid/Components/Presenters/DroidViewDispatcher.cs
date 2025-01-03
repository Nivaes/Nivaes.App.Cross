namespace Nivaes.App.Cross.Droid
{
    using Nivaes.App.Cross.Presenters;

    public sealed class DroidViewDispatcher : ViewDispatcher, IViewDispatcher
    {
        private readonly IViewPresenter mViewPresenter;

        public DroidViewDispatcher(IViewPresenter viewPresenter) 
            : base()
        {
            mViewPresenter = viewPresenter;
        }

        public override bool IsOnMainThread => Application.SynchronizationContext == SynchronizationContext.Current;

        public override Task<bool> ShowViewModelOnMainThread(IViewModelRequest request)
        {
            return mViewPresenter.Show(request);
        }

        public override Task<bool> ShowViewModelOnBackgroundThread(IViewModelRequest request, Func<IViewModelRequest, Task<bool>> action)
        {
            var result = false;

            Application.SynchronizationContext.Post(async ignored =>
            {
                result = await mViewPresenter.Show(request);
            }, null);

            return Task.FromResult(true);
        }
    }
}
