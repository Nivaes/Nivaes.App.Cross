namespace Nivaes.App.Cross.UIKitLib
{
    using System.Linq;
    using Foundation;
    using ObjCRuntime;
    using UIKit;

    public class SplitViewController
        : MvxBaseSplitViewController, IMvxSplitViewController
    {
        public SplitViewController() : base()
        {
        }

        public SplitViewController(NSCoder coder) : base(coder)
        {
        }

        protected SplitViewController(NSObjectFlag t) : base(t)
        {
        }

        protected internal SplitViewController(NativeHandle handle) : base(handle)
        {
        }

        public SplitViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        public SplitViewController(UISplitViewControllerStyle style) : base(style)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            PreferredDisplayMode = UISplitViewControllerDisplayMode.AllVisible;
        }

        public virtual void ShowDetailView(UIViewController viewController, SplitViewPresentationAttribute attribute)
        {
            viewController = attribute.WrapInNavigationController ? new NavigationController(viewController) : viewController;

            ShowDetailViewController(viewController, this);
        }

        public virtual void ShowMasterView(UIViewController viewController, SplitViewPresentationAttribute attribute)
        {
            var newStack = ViewControllers.ToList();

            viewController = attribute.WrapInNavigationController ? new NavigationController(viewController) : viewController;

            if (newStack.Any())
                newStack.RemoveAt(0);

            newStack.Insert(0, viewController);

            ViewControllers = newStack.ToArray();
        }

        public virtual bool CloseChildViewModel(ICrossViewModel viewModel, BasePresentationAttribute attribute)
        {
            if (!ViewControllers.Any())
                return false;

            var toClose =
                ViewControllers
                    .Select(v => v.GetIMvxIosView())
                    .FirstOrDefault(mvxView => mvxView?.ViewModel == viewModel);

            if (toClose != null)
            {
                var newStack = ViewControllers.Where(v => v.GetIMvxIosView() != toClose);
                ViewControllers = newStack.ToArray();

                return true;
            }

            return false;
        }
    }

    public class MvxSplitViewController<TViewModel>
        : SplitViewController, IMvxIosView<TViewModel>
        where TViewModel : class, ICrossViewModel
    {
        public MvxSplitViewController()
        {
        }

        public MvxSplitViewController(NSCoder coder) : base(coder)
        {
        }

        public MvxSplitViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        protected MvxSplitViewController(NSObjectFlag t) : base(t)
        {
        }

        protected internal MvxSplitViewController(NativeHandle handle) : base(handle)
        {
        }

        public new TViewModel ViewModel
        {
            get { return (TViewModel)base.ViewModel; }
            set { base.ViewModel = value; }
        }

        public CrossFluentBindingDescriptionSet<IMvxIosView<TViewModel>, TViewModel> CreateBindingSet()
        {
            return this.CreateBindingSet<IMvxIosView<TViewModel>, TViewModel>();
        }
    }
}
