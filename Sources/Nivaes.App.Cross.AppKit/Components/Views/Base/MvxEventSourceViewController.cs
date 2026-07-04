namespace Nivaes.App.Cross.AppKitLib
{
    using Nivaes.App.Cross;
    using ObjCRuntime;

    public class MvxEventSourceViewController
        : NSViewController, IMvxEventSourceViewController
    {
        protected MvxEventSourceViewController()
        {
            this.Initialize();
        }

        protected MvxEventSourceViewController(NativeHandle handle)
            : base(handle)
        {
            this.Initialize();
        }

        protected MvxEventSourceViewController(NSCoder coder)
            : base(coder)
        {
            this.Initialize();
        }

        protected MvxEventSourceViewController(string nibName, NSBundle bundle)
            : base(nibName, bundle)
        {
            this.Initialize();
        }

        private void Initialize()
        {
            // Method intentionally left empty.
        }

        public override void LoadView()
        {
            base.LoadView();
            //this.ViewDidLoad();
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ViewDidLoadCalled?.Raise(this);
        }

        public override void ViewDidLayout()
        {
            base.ViewDidLayout();
            ViewDidLayoutCalled?.Raise(this);
        }

        public override void ViewWillAppear()
        {
            base.ViewWillAppear();
            ViewWillAppearCalled?.Raise(this);
        }

        public override void ViewDidAppear()
        {
            base.ViewDidAppear();
            ViewDidAppearCalled?.Raise(this);
        }

        public override void ViewWillDisappear()
        {
            base.ViewWillDisappear();
            ViewWillDisappearCalled?.Raise(this);
        }

        public override void ViewDidDisappear()
        {
            base.ViewDidDisappear();
            ViewDidDisappearCalled?.Raise(this);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DisposeCalled.Raise(this);
            }
            base.Dispose(disposing);
        }

        public event EventHandler ViewDidLoadCalled;

        public event EventHandler ViewDidLayoutCalled;

        public event EventHandler ViewWillAppearCalled;

        public event EventHandler ViewDidAppearCalled;

        public event EventHandler ViewDidDisappearCalled;

        public event EventHandler ViewWillDisappearCalled;

        public event EventHandler DisposeCalled;
    }
}
