using ObjCRuntime;

namespace Nivaes.App.Cross.UIKitLib
{
    public class MvxBasePageViewController<TViewModel>
        : MvxEventSourcePageViewController, IMvxIosView<TViewModel>, IMvxIosView
        where TViewModel : class, ICrossViewModel
    {
        #region Constructors
        public MvxBasePageViewController(UIPageViewControllerTransitionStyle style = UIPageViewControllerTransitionStyle.Scroll, UIPageViewControllerNavigationOrientation navigationOrientation = UIPageViewControllerNavigationOrientation.Horizontal, UIPageViewControllerSpineLocation spineLocation = UIPageViewControllerSpineLocation.None) : base(style, navigationOrientation, spineLocation)
        {
            this.AdaptForBinding();
        }

        public MvxBasePageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation, UIPageViewControllerSpineLocation spineLocation, float interPageSpacing) : base(style, navigationOrientation, spineLocation, interPageSpacing)
        {
            this.AdaptForBinding();
        }

        public MvxBasePageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation) : base(style, navigationOrientation)
        {
            this.AdaptForBinding();
        }

        public MvxBasePageViewController(NSCoder coder) : base(coder)
        {
            this.AdaptForBinding();
        }

        protected MvxBasePageViewController(NSObjectFlag t) : base(t)
        {
            this.AdaptForBinding();
        }

        protected internal MvxBasePageViewController(NativeHandle handle) : base(handle)
        {
            this.AdaptForBinding();
        }

        public MvxBasePageViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
            this.AdaptForBinding();
        }

        public MvxBasePageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation, NSDictionary options) : base(style, navigationOrientation, options)
        {
            this.AdaptForBinding();
        }
        #endregion

        #region Data
        public ICrossBindingContext? BindingContext { get; set; }

        public object? DataContext
        {
            get
            {
                // special code needed in PageViewController because View is initialized during construction
                return BindingContext?.DataContext;
            }
            set
            {
                BindingContext?.DataContext = value;
            }
        }

        public TViewModel? ViewModel
        {
            get { return DataContext as TViewModel; }
            set { DataContext = value; }
        }

        ICrossViewModel? ICrossView.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (TViewModel?)value;
        }
        #endregion

        public CrossViewModelRequest? Request { get; set; }


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
