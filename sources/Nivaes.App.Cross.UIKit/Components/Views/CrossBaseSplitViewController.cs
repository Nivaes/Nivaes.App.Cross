namespace Nivaes.App.Cross.UIKit
{
    using ObjCRuntime;

    public class CrossBaseSplitViewController : 
        CrossEventSourceSplitViewController, 
        ICrossIosView
    {
        public CrossBaseSplitViewController() : base()
        {
            this.AdaptForBinding();
        }

        public CrossBaseSplitViewController(NSCoder coder) : base(coder)
        {
            this.AdaptForBinding();
        }

        protected CrossBaseSplitViewController(NSObjectFlag t) : base(t)
        {
            this.AdaptForBinding();
        }

        protected internal CrossBaseSplitViewController(NativeHandle handle) : base(handle)
        {
            this.AdaptForBinding();
        }

        public CrossBaseSplitViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
            this.AdaptForBinding();
        }

        public CrossBaseSplitViewController(UISplitViewControllerStyle style) : base(style)
        {
            this.AdaptForBinding();
        }

        public object DataContext
        {
            get { return BindingContext.DataContext; }
            set { BindingContext.DataContext = value; }
        }

        public ICrossViewModel ViewModel
        {
            get { return DataContext as ICrossViewModel; }
            set { DataContext = value; }
        }

        public ICrossViewModelRequest Request { get; set; }

        public ICrossBindingContext BindingContext { get; set; }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            ViewModel?.ViewCreated();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);
            ViewModel?.ViewAppearing();
        }

        public override void ViewDidAppear(bool animated)
        {
            base.ViewDidAppear(animated);
            ViewModel?.ViewAppeared();
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);
            ViewModel?.ViewDisappearing();
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);
            ViewModel?.ViewDisappeared();
        }

        public override void DidMoveToParentViewController(UIViewController parent)
        {
            base.DidMoveToParentViewController(parent);
            if (parent == null)
                ViewModel?.ViewDestroy();
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject sender)
        {
            base.PrepareForSegue(segue, sender);
            this.ViewModelRequestForSegue(segue, sender);
        }
    }

    public class MvxBaseSplitViewController<TViewModel> : 
        CrossBaseSplitViewController, 
        ICrossIosView<TViewModel>
        where TViewModel : class, ICrossViewModel
    {
        public MvxBaseSplitViewController()
        {
        }

        public MvxBaseSplitViewController(NSCoder coder) : base(coder)
        {
        }

        public MvxBaseSplitViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        protected MvxBaseSplitViewController(NSObjectFlag t) : base(t)
        {
        }

        protected internal MvxBaseSplitViewController(NativeHandle handle) : base(handle)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public CrossFluentBindingDescriptionSet<ICrossIosView<TViewModel>, TViewModel> CreateBindingSet()
        {
            return this.CreateBindingSet<ICrossIosView<TViewModel>, TViewModel>();
        }
    }
}
