namespace Nivaes.App.Cross.Droid
{
    public sealed class CrossDroidViewDispatcher : CrossViewDispatcher, ICrossViewDispatcher
    {
        private readonly ICrossViewPresenter mViewPresenter;

        public CrossDroidViewDispatcher(ICrossViewPresenter viewPresenter) 
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
