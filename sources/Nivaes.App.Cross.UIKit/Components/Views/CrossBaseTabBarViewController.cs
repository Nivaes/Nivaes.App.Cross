namespace Nivaes.App.Cross.UIKit
{
    using ObjCRuntime;

    public class CrossBaseTabBarViewController : 
        CrossEventSourceTabBarController, 
        ICrossIosView
    {
        public CrossBaseTabBarViewController() : base()
        {
            this.AdaptForBinding();
        }

        public CrossBaseTabBarViewController(NSCoder coder) : base(coder)
        {
            this.AdaptForBinding();
        }

        protected CrossBaseTabBarViewController(NSObjectFlag t) : base(t)
        {
            this.AdaptForBinding();
        }

        protected internal CrossBaseTabBarViewController(NativeHandle handle) : base(handle)
        {
            this.AdaptForBinding();
        }

        public CrossBaseTabBarViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
            this.AdaptForBinding();
        }

        public object DataContext
        {
            get
            {
                // special code needed in TabBar because View is initialized during construction
                return BindingContext?.DataContext;
            }
            set
            {
                BindingContext.DataContext = value;
            }
        }

        public ICrossViewModel ViewModel
        {
            get { return DataContext as ICrossViewModel; }
            set { DataContext = value; }
        }

        public ICrossViewModelRequest Request { get; set; }

        public ICrossBindingContext BindingContext { get; set; }
        ICrossViewModelRequest ICrossIosView.Request { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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

    public class MvxBaseTabBarViewController<TViewModel> : CrossBaseTabBarViewController, ICrossIosView<TViewModel>
        where TViewModel : class, ICrossViewModel
    {
        public MvxBaseTabBarViewController()
        {
        }

        public MvxBaseTabBarViewController(NSCoder coder) : base(coder)
        {
        }

        public MvxBaseTabBarViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        protected MvxBaseTabBarViewController(NSObjectFlag t) : base(t)
        {
        }

        protected internal MvxBaseTabBarViewController(NativeHandle handle) : base(handle)
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
