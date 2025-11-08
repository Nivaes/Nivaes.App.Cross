namespace Nivaes.App.Cross.UIKit
{
    using ObjCRuntime;

    public class CrossPageViewController : CrossBasePageViewController, ICrossPageViewController
    {
        public CrossPageViewController(UIPageViewControllerTransitionStyle style = UIPageViewControllerTransitionStyle.Scroll, UIPageViewControllerNavigationOrientation navigationOrientation = UIPageViewControllerNavigationOrientation.Horizontal, UIPageViewControllerSpineLocation spineLocation = UIPageViewControllerSpineLocation.None) : base(style, navigationOrientation, spineLocation)
        {
        }

        public CrossPageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation, UIPageViewControllerSpineLocation spineLocation, float interPageSpacing) : base(style, navigationOrientation, spineLocation, interPageSpacing)
        {
        }

        public CrossPageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation) : base(style, navigationOrientation)
        {
        }

        public CrossPageViewController(NSCoder coder) : base(coder)
        {
        }

        protected CrossPageViewController(NSObjectFlag t) : base(t)
        {
        }

        protected internal CrossPageViewController(NativeHandle handle) : base(handle)
        {
        }

        public CrossPageViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        public CrossPageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation, NSDictionary options) : base(style, navigationOrientation, options)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            GetNextViewController = (pc, rc) => GetNextViewControllerPage(rc);
            GetPreviousViewController = (pc, rc) => GetPreviousViewControllerPage(rc);
        }

        public IList<UIViewController> Pages { get; protected set; } = new List<UIViewController>();

        public virtual bool IsFirstPage(UIViewController viewController) => Pages.IndexOf(viewController) == 0;

        public virtual bool IsLastPage(UIViewController viewController) => Pages.IndexOf(viewController) == Pages.Count - 1;

        protected virtual UIViewController GetNextViewControllerPage(UIViewController rc) => IsLastPage(rc) ? null : Pages[Pages.IndexOf(rc) + 1];

        protected virtual UIViewController GetPreviousViewControllerPage(UIViewController rc) => IsFirstPage(rc) ? null : Pages[Pages.IndexOf(rc) - 1];

        public virtual void AddPage(UIViewController viewController, CrossPagePresentationAttribute attribute)
        {
            // add Page
            Pages.Add(viewController);

            // Start the ui page view controller when we add the first page
            if (Pages.Count == 1)
            {
                SetViewControllers(Pages.ToArray(), UIPageViewControllerNavigationDirection.Forward, true, null);
            }
        }

        public virtual bool RemovePage(ICrossViewModel viewModel)
        {
            if (Pages == null || !Pages.Any())
                return false;

            var pageToClose = Pages.Where(v => !(v is UINavigationController))
                                              .Select(v => v.GetIMvxIosView())
                                              .FirstOrDefault(mvxView => mvxView.ViewModel == viewModel);

            if (pageToClose != null)
            {
                Pages = Pages.Where(v => v != pageToClose).ToList();
                return true;
            }

            return false;
        }
    }

    public class MvxPageViewController<TViewModel> : 
        CrossPageViewController, 
        ICrossIosView<TViewModel>
        where TViewModel : class, ICrossViewModel
    {
        public MvxPageViewController()
        {
        }

        public MvxPageViewController(NSCoder coder) : base(coder)
        {
        }

        public MvxPageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation) : base(style, navigationOrientation)
        {
        }

        public MvxPageViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        public MvxPageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation, UIPageViewControllerSpineLocation spineLocation) : base(style, navigationOrientation, spineLocation)
        {
        }

        public MvxPageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation, NSDictionary options) : base(style, navigationOrientation, options)
        {
        }

        public MvxPageViewController(UIPageViewControllerTransitionStyle style, UIPageViewControllerNavigationOrientation navigationOrientation, UIPageViewControllerSpineLocation spineLocation, float interPageSpacing) : base(style, navigationOrientation, spineLocation, interPageSpacing)
        {
        }

        protected MvxPageViewController(NSObjectFlag t) : base(t)
        {
        }

        protected internal MvxPageViewController(NativeHandle handle) : base(handle)
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
