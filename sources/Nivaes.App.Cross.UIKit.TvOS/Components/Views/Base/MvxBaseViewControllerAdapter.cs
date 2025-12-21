namespace MvvmCross.Platforms.Tvos.Views.Base
{
    using System;
    using MvvmCross.Base;
    using Nivaes.App.Cross;
    using UIKit;

    public class MvxBaseViewControllerAdapter
    {
        private readonly IMvxEventSourceViewController _eventSource;

        protected UIViewController ViewController => _eventSource as UIViewController;

        public MvxBaseViewControllerAdapter(IMvxEventSourceViewController eventSource)
        {
            if (eventSource == null)
                throw new ArgumentException("eventSource - eventSource should not be null");

            if (!(eventSource is UIViewController))
                throw new ArgumentException("eventSource - eventSource should be a UIViewController");

            _eventSource = eventSource;
            _eventSource.ViewDidAppearCalled += HandleViewDidAppearCalled;
            _eventSource.ViewDidDisappearCalled += HandleViewDidDisappearCalled;
            _eventSource.ViewWillAppearCalled += HandleViewWillAppearCalled;
            _eventSource.ViewWillDisappearCalled += HandleViewWillDisappearCalled;
            _eventSource.DisposeCalled += HandleDisposeCalled;
            _eventSource.ViewDidLoadCalled += HandleViewDidLoadCalled;
            _eventSource.ViewDidLayoutSubviewsCalled += HandleViewDidLayoutSubviewsCalled;
        }

        public virtual void HandleViewDidLoadCalled(object sender, EventArgs e)
        {
        }

        public virtual void HandleViewDidLayoutSubviewsCalled(object sender, EventArgs e)
        {
        }

        public virtual void HandleDisposeCalled(object sender, EventArgs e)
        {
        }

        public virtual void HandleViewWillDisappearCalled(object sender, CrossValueEventArgs<bool> e)
        {
        }

        public virtual void HandleViewWillAppearCalled(object sender, CrossValueEventArgs<bool> e)
        {
        }

        public virtual void HandleViewDidDisappearCalled(object sender, CrossValueEventArgs<bool> e)
        {
        }

        public virtual void HandleViewDidAppearCalled(object sender, CrossValueEventArgs<bool> e)
        {
        }
    }
}
