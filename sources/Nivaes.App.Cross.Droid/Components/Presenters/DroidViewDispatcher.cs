namespace Nivaes.App.Cross.Droid
{
    using Nivaes.App.Cross.Presenters;

    public sealed class DroidViewDispatcher : CrossViewDispatcher, ICrossViewDispatcher
    {
        private readonly ICrossViewPresenter mViewPresenter;

        public DroidViewDispatcher(ICrossViewPresenter viewPresenter) 
            : base()
        {
            mViewPresenter = viewPresenter;
        }

        public override bool IsOnMainThread => Application.SynchronizationContext == SynchronizationContext.Current;

        public override Task<bool> ShowViewModelOnMainThread(ICrossViewModelRequest request)
        {
            return mViewPresenter.Show(request);
        }

        public override Task<bool> ShowViewModelOnBackgroundThread(ICrossViewModelRequest request, Func<ICrossViewModelRequest, Task<bool>> action)
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
