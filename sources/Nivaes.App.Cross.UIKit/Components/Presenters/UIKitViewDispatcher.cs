namespace Nivaes.App.Cross.UIKit
{
    using System;
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Presenters;

    public sealed class UIKitViewDispatcher 
        : ViewDispatcher, IViewDispatcher
    {
        private readonly SynchronizationContext mSynchronizationContext;

        private readonly IViewPresenter mViewPresenter;

        public UIKitViewDispatcher(IViewPresenter viewPresenter, AppDataModel appDatamodel) : base()
        {
            if (SynchronizationContext.Current == null)
                throw new CrossException("SynchronizationContext must not be null - check to make sure Dispatcher is created on UI thread");

            mSynchronizationContext = SynchronizationContext.Current;

            mViewPresenter = viewPresenter;
        }

        override public bool IsOnMainThread => mSynchronizationContext == SynchronizationContext.Current;

        public override Task<bool> ShowViewModelOnMainThread(IViewModelRequest request)
        {
            return mViewPresenter.Show(request);
        }

        public override Task<bool> ShowViewModelOnBackgroundThread(IViewModelRequest request, Func<IViewModelRequest, Task<bool>> action)
        {
            var result = false;
            UIApplication.SharedApplication.BeginInvokeOnMainThread(async() =>
            {
                result = await action(request);
            });

            return Task.FromResult(true);
        }
    }
}
