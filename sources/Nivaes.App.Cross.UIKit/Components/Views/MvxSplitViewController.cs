namespace MvvmCross.Platforms.Ios.Views
{
    using System;
    using System.Linq;
    using Foundation;
    using MvvmCross.Binding.BindingContext;
    using MvvmCross.Platforms.Ios.Presenters.Attributes;
    using MvvmCross.Presenters.Attributes;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;
    using ObjCRuntime;
    using UIKit;

    public class MvxSplitViewController
        : MvxBaseSplitViewController, IMvxSplitViewController
    {
        public MvxSplitViewController() : base()
        {
        }

        public MvxSplitViewController(NSCoder coder) : base(coder)
        {
        }

        protected MvxSplitViewController(NSObjectFlag t) : base(t)
        {
        }

        protected internal MvxSplitViewController(NativeHandle handle) : base(handle)
        {
        }

        public MvxSplitViewController(string nibName, NSBundle bundle) : base(nibName, bundle)
        {
        }

        public MvxSplitViewController(UISplitViewControllerStyle style) : base(style)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            PreferredDisplayMode = UISplitViewControllerDisplayMode.AllVisible;
        }

        public virtual void ShowDetailView(UIViewController viewController, MvxSplitViewPresentationAttribute attribute)
        {
            viewController = attribute.WrapInNavigationController ? new MvxNavigationController(viewController) : viewController;

            ShowDetailViewController(viewController, this);
        }

        public virtual void ShowMasterView(UIViewController viewController, MvxSplitViewPresentationAttribute attribute)
        {
            var newStack = ViewControllers.ToList();

            viewController = attribute.WrapInNavigationController ? new MvxNavigationController(viewController) : viewController;

            if (newStack.Any())
                newStack.RemoveAt(0);

            newStack.Insert(0, viewController);

            ViewControllers = newStack.ToArray();
        }

        public virtual bool CloseChildViewModel(ICrossViewModel viewModel, MvxBasePresentationAttribute attribute)
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
        : MvxSplitViewController, IMvxIosView<TViewModel>
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

        public MvxFluentBindingDescriptionSet<IMvxIosView<TViewModel>, TViewModel> CreateBindingSet()
        {
            return this.CreateBindingSet<IMvxIosView<TViewModel>, TViewModel>();
        }
    }
}
