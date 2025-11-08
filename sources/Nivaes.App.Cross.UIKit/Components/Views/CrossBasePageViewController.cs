namespace Nivaes.App.Cross.UIKit
{
    public class CrossBasePageViewController : CrossEventSourcePageViewController, ICrossIosView
    {
        public CrossBasePageViewController(UIPageViewControllerTransitionStyle style = UIPageViewControllerTransitionStyle.Scroll, UIPageViewControllerNavigationOrientation navigationOrientation = UIPageViewControllerNavigationOrientation.Horizontal, UIPageViewControllerSpineLocation spineLocation = UIPageViewControllerSpineLocation.None) : base(style, navigationOrientation, spineLocation)
        {
            this.AdaptForBinding();
        }

        public CrossBasePageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation, UIPageViewControllerSpineLocation spineLocation, float interPageSpacing) : base(style, navigationOrientation, spineLocation, interPageSpacing)
        {
            this.AdaptForBinding();
        }

        public CrossBasePageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation) : base(style, navigationOrientation)
        {
            this.AdaptForBinding();
        }

        public CrossBasePageViewController(NSCoder coder) : base(coder)
        {
            this.AdaptForBinding();
        }

        protected CrossBasePageViewController(NSObjectFlag t) : base(t)
        {
            this.AdaptForBinding();
        }

        protected internal CrossBasePageViewController(NativeHandle handle) : base(handle)
        {
            this.AdaptForBinding();
        }

        public CrossBasePageViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
            this.AdaptForBinding();
        }

        public CrossBasePageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation, NSDictionary options) : base(style, navigationOrientation, options)
        {
            this.AdaptForBinding();
        }
        public object DataContext
        {
            get
            {
                // special code needed in PageViewController because View is initialized during construction
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

        public CrossViewModelRequest Request { get; set; }

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

    public class MvxBasePageViewController<TViewModel> : CrossPageViewController, IMvxIosView<TViewModel> where TViewModel : class, IMvxViewModel
    {
        public MvxBasePageViewController()
        {
        }

        public MvxBasePageViewController(NSCoder coder) : base(coder)
        {
        }

        public MvxBasePageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation) : base(style, navigationOrientation)
        {
        }

        public MvxBasePageViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        public MvxBasePageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation, UIPageViewControllerSpineLocation spineLocation) : base(style, navigationOrientation, spineLocation)
        {
        }

        public MvxBasePageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation, NSDictionary options) : base(style, navigationOrientation, options)
        {
        }

        public MvxBasePageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation, UIPageViewControllerSpineLocation spineLocation, float interPageSpacing) : base(style, navigationOrientation, spineLocation, interPageSpacing)
        {
        }

        protected MvxBasePageViewController(NSObjectFlag t) : base(t)
        {
        }

        protected internal MvxBasePageViewController(NativeHandle handle) : base(handle)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public MvxFluentBindingDescriptionSet<IMvxIosView<TViewModel>, TViewModel> CreateBindingSet()
        {
            return this.CreateBindingSet<IMvxIosView<TViewModel>, TViewModel>();
        }
    }
}
