namespace Nivaes.App.Cross.UIKit
{
    using ObjCRuntime;

    public class CrossSplitViewController : 
        CrossBaseSplitViewController, 
        ICrossSplitViewController
    {
        public CrossSplitViewController() : base()
        {
        }

        public CrossSplitViewController(NSCoder coder) : base(coder)
        {
        }

        protected CrossSplitViewController(NSObjectFlag t) : base(t)
        {
        }

        protected internal CrossSplitViewController(NativeHandle handle) : base(handle)
        {
        }

        public CrossSplitViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        public CrossSplitViewController(UISplitViewControllerStyle style) : base(style)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            PreferredDisplayMode = UISplitViewControllerDisplayMode.AllVisible;
        }

        public virtual void ShowDetailView(UIViewController viewController, CrossSplitViewPresentationAttribute attribute)
        {
            viewController = attribute.WrapInNavigationController ? new CrossNavigationController(viewController) : viewController;

            ShowDetailViewController(viewController, this);
        }

        public virtual void ShowMasterView(UIViewController viewController, CrossSplitViewPresentationAttribute attribute)
        {
            var newStack = ViewControllers.ToList();

            viewController = attribute.WrapInNavigationController ? new CrossNavigationController(viewController) : viewController;

            if (newStack.Any())
                newStack.RemoveAt(0);

            newStack.Insert(0, viewController);

            ViewControllers = newStack.ToArray();
        }

        public virtual bool CloseChildViewModel(ICrossViewModel viewModel, CrossBasePresentationAttribute attribute)
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

    public class MvxSplitViewController<TViewModel> : CrossSplitViewController, ICrossIosView<TViewModel>
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
            return this.CreateBindingSet<ICrossIosView<TViewModel>, TViewModel>();
        }
    }
}
