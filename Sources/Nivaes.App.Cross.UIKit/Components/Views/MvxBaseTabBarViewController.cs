using ObjCRuntime;

namespace Nivaes.App.Cross.UIKitLib
{
    public class MvxBaseTabBarViewController<TViewModel>
      : MvxEventSourceTabBarController, IMvxIosView<TViewModel>, IMvxIosView
      where TViewModel : class, ICrossViewModel
    {
        #region Constructors
        public MvxBaseTabBarViewController() : base()
        {
            this.AdaptForBinding();
        }

        public MvxBaseTabBarViewController(NSCoder coder) : base(coder)
        {
            this.AdaptForBinding();
        }

        protected MvxBaseTabBarViewController(NSObjectFlag t) : base(t)
        {
            this.AdaptForBinding();
        }

        protected internal MvxBaseTabBarViewController(NativeHandle handle) : base(handle)
        {
            this.AdaptForBinding();
        }

        public MvxBaseTabBarViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
            this.AdaptForBinding();
        }
        #endregion

        #region Data
        public ICrossBindingContext? BindingContext
        {
            get;
            set;
        }

        public object? DataContext
        {
            get
            {
                // special code needed in TabBar because View is initialized during construction
                return BindingContext?.DataContext;
            }
            set
            {
                BindingContext?.DataContext = value;
            }
        }

        public TViewModel? ViewModel
        {
            get { return (TViewModel?)DataContext; }
            set
            {
                DataContext = value;
            }
        }

        ICrossViewModel? ICrossView.ViewModel
        {
            get => this.ViewModel;
            set => ViewModel = (TViewModel?)value;
        }
        #endregion

        public ViewModelRequest? Request { get; set; }


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

        public override void DidMoveToParentViewController(UIViewController? parent)
        {
            base.DidMoveToParentViewController(parent);
            if (parent == null)
                ViewModel?.ViewDestroy();
        }

        public override void PrepareForSegue(UIStoryboardSegue segue, NSObject? sender)
        {
            base.PrepareForSegue(segue, sender);
            this.ViewModelRequestForSegue(segue, sender);
        }

        public CrossFluentBindingDescriptionSet<IMvxIosView<TViewModel>, TViewModel> CreateBindingSet()
        {
            return this.CreateBindingSet<IMvxIosView<TViewModel>, TViewModel>();
        }
    }
}
