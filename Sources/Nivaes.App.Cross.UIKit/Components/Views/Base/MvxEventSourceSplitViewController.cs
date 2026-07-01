namespace Nivaes.App.Cross.UIKitOS
{
    using System;
    using Foundation;
    using ObjCRuntime;

    public class MvxEventSourceSplitViewController
        : UISplitViewController, IMvxEventSourceViewController
    {
        public MvxEventSourceSplitViewController() : base()
        {
        }

        public MvxEventSourceSplitViewController(NSCoder coder) : base(coder)
        {
        }

        protected MvxEventSourceSplitViewController(NSObjectFlag t) : base(t)
        {
        }

        protected internal MvxEventSourceSplitViewController(NativeHandle handle) : base(handle)
        {
        }

        public MvxEventSourceSplitViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        public MvxEventSourceSplitViewController(UISplitViewControllerStyle style) : base(style)
        {
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            ViewWillDisappearCalled.Raise(this, animated);
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            ViewDidAppearCalled.Raise(this, animated);
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ViewWillAppearCalled.Raise(this, animated);
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            ViewDidDisappearCalled.Raise(this, animated);
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ViewDidLoadCalled.Raise(this);
        }

        public override void ViewDidLayoutSubviews()
        {
            base.ViewDidLayoutSubviews();
            ViewDidLayoutSubviewsCalled.Raise(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeCalled.Raise(this);
            }
            base.Dispose(disposing);
        }

        public event EventHandler ViewDidLoadCalled;

        public event EventHandler ViewDidLayoutSubviewsCalled;

        public event EventHandler<CrossValueEventArgs<bool>> ViewWillAppearCalled;

        public event EventHandler<CrossValueEventArgs<bool>> ViewDidAppearCalled;

        public event EventHandler<CrossValueEventArgs<bool>> ViewDidDisappearCalled;

        public event EventHandler<CrossValueEventArgs<bool>> ViewWillDisappearCalled;

        public event EventHandler DisposeCalled;
    }
}
