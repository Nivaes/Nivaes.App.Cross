using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public class IosViewPresenterManager
        : CrossViewPresenterManager, IIosViewPresenterManager
    {
        private readonly IPressenterActionContext Context;

        private readonly IMvxIosViewCreator _viewCreator;

        public UINavigationController? MasterNavigationController { get; protected set; }

#if IOS || MACCATALYST
        public UIViewController? PopoverViewController { get; protected set; }
#endif

        public List<UIViewController> ModalViewControllers { get; } = [];

        //public IMvxTabBarViewController? TabBarViewController { get; protected set; }

        public IMvxPageViewController? PageViewController { get; protected set; }

        public IMvxSplitViewController? SplitViewController { get; protected set; }

        public IosViewPresenterManager(
                    IPressenterActionContext context,
                    IMvxIosViewCreator viewCreator, ILogger<IosViewPresenterManager> logger)
            : base(logger)
        {
            Context = context;
            _viewCreator = viewCreator;
        }

        public override BasePresentationAttribute CreatePresentationAttribute(Type? viewModelType, Type? viewType)
        {
            if (MasterNavigationController == null &&
                Context.TabBarViewController?.CanShowChildView() != true)
            {
                Logger?.LogTrace(
                    "PresentationAttribute nor MasterNavigationController found for {ViewTypeName}. Assuming Root presentation",
                    viewType?.Name);

                return new RootPresentationAttribute
                {
                    WrapInNavigationController = true,
                    ViewType = viewType,
                    ViewModelType = viewModelType
                };
            }

            Logger?.LogTrace(
                "PresentationAttribute not found for {ViewTypeName}. Assuming animated Child presentation", viewType?.Name);

            return new ChildPresentationAttribute { ViewType = viewType, ViewModelType = viewModelType };
        }

        public override object? CreateOverridePresentationAttributeViewInstance(Type viewType)
        {
            return (UIViewController?)_viewCreator.CreateViewOfType(viewType);
        }      

        public override ValueTask<bool> ChangePresentation(CrossPresentationHint hint)
        {
            if (hint is CrossPagePresentationHint pagePresentationHint)
            {
                if (ChangePagePresentation(pagePresentationHint))
                {
                    return ValueTask.FromResult(true);
                }
            }

            return base.ChangePresentation(hint);
        }

        private bool ChangePagePresentation(CrossPagePresentationHint pagePresentationHint)
        {
            if (!(Context.TabBarViewController is UITabBarController tabsController) ||
                tabsController.ViewControllers == null)
            {
                return false;
            }

            foreach (var vc in tabsController.ViewControllers)
            {
                IMvxIosView? tabView;

                if (vc is UINavigationController navigationController)
                {
                    var root = navigationController.ViewControllers?.FirstOrDefault();
                    tabView = root.GetIMvxIosView();
                }
                else
                {
                    tabView = vc.GetIMvxIosView();
                }

                var viewModelType = tabView.GetViewModelType();
                if (viewModelType == null || viewModelType != pagePresentationHint.ViewModel) continue;

                tabsController.SelectedViewController = vc;
                return true;
            }

            return false;
        }

        protected virtual Task<bool> ShowChildViewController(
            UIViewController viewController,
            ChildPresentationAttribute attribute,
            ViewModelRequest request)
        {
            ArgumentNullException.ThrowIfNull(viewController);
            ArgumentNullException.ThrowIfNull(attribute);

            if (viewController is IMvxSplitViewController)
                throw new AppException("A SplitViewController cannot be presented as a child. Consider using Root instead");

#if IOS || MACCATALYST
            if (PopoverViewController != null)
            {
                return ShowPopoverViewControllerChild(viewController, attribute);
            }
#endif

            if (ModalViewControllers.Count > 0)
            {
                return ShowModalViewControllerChild(viewController, attribute);
            }

            if (Context.TabBarViewController != null && Context.TabBarViewController.ShowChildView(viewController))
            {
                return Task.FromResult(true);
            }

            if (MasterNavigationController != null)
            {
                PushViewControllerIntoStack(MasterNavigationController, viewController, attribute);
                return Task.FromResult(true);
            }

            throw new AppException($"Trying to show View type: {viewController.GetType().Name} as child, but there is no current stack!");
        }

        [Obsolete]
        private Task<bool> ShowModalViewControllerChild(UIViewController viewController, ChildPresentationAttribute attribute)
        {
            if (ModalViewControllers.LastOrDefault() is UINavigationController modalNavController)
            {
                PushViewControllerIntoStack(modalNavController, viewController, attribute);

                return Task.FromResult(true);
            }

            throw new AppException(
                $"Trying to show View type: {viewController.GetType().Name} as child, but there is currently a plain modal view presented!");
        }

#if IOS || MACCATALYST
        [Obsolete]
        private Task<bool> ShowPopoverViewControllerChild(UIViewController viewController, ChildPresentationAttribute attribute)
        {
            if (PopoverViewController is UINavigationController popoverNavController)
            {
                PushViewControllerIntoStack(popoverNavController, viewController, attribute);

                return Task.FromResult(true);
            }

            throw new AppException(
                $"Trying to show View type: {viewController.GetType().Name} as child, but there is currently a plain popover view presented!");
        }
#endif

        [Obsolete("", true)]
        protected virtual Task<bool> ShowTabViewController(
            UIViewController viewController,
            TabPresentationAttribute attribute,
            ViewModelRequest request)
        {
            ArgumentNullException.ThrowIfNull(viewController);
            ArgumentNullException.ThrowIfNull(attribute);

            if (Context.TabBarViewController == null)
                throw new AppException("Trying to show a tab without a TabBarViewController, this is not possible!");

            if (viewController is ITabBarItemViewController tabBarItem)
            {
                attribute.TabName = tabBarItem.TabName;
                attribute.TabIconName = tabBarItem.TabIconName;
                attribute.TabSelectedIconName = tabBarItem.TabSelectedIconName;
            }

            if (attribute.WrapInNavigationController)
                viewController = CreateNavigationController(viewController);

            Context.TabBarViewController.ShowTabView(
                viewController,
                attribute);
            return Task.FromResult(true);
        }

        protected virtual Task<bool> ShowPageViewController(
            UIViewController viewController,
            PagePresentationAttribute attribute,
            ViewModelRequest request)
        {
            ArgumentNullException.ThrowIfNull(viewController);
            ArgumentNullException.ThrowIfNull(attribute);

            if (PageViewController == null)
                throw new AppException("Trying to show a page without a PageViewController, this is not possible!");

            if (attribute.WrapInNavigationController)
                viewController = CreateNavigationController(viewController);

            PageViewController.AddPage(
                viewController,
                attribute);
            return Task.FromResult(true);
        }

        private UIViewController GetParentViewController()
        {
            //Ensure to get a ViewController that is not being dismissed. See related bugs https://github.com/MvvmCross/MvvmCross/issues/4781
            return ModalViewControllers.LastOrDefault(x => !x.IsBeingDismissed) ?? Context.Window.RootViewController
                ?? throw new AppException($"No parent ViewController found.");
        }

        protected virtual Task<bool> ShowMasterSplitViewController(
            UIViewController viewController,
            SplitViewPresentationAttribute attribute,
            ViewModelRequest request)
        {
            ArgumentNullException.ThrowIfNull(viewController);
            ArgumentNullException.ThrowIfNull(attribute);

            if (SplitViewController == null)
                throw new AppException("Trying to show a master page without a SplitViewController, this is not possible!");

            SplitViewController.ShowMasterView(viewController, attribute);
            return Task.FromResult(true);
        }

        [Obsolete]
        protected virtual Task<bool> ShowDetailSplitViewController(
            UIViewController viewController,
            SplitViewPresentationAttribute attribute,
            ViewModelRequest request)
        {
            ArgumentNullException.ThrowIfNull(viewController);
            ArgumentNullException.ThrowIfNull(attribute);

            if (SplitViewController == null)
                throw new AppException("Trying to show a detail page without a SplitViewController, this is not possible!");

            SplitViewController.ShowDetailView(viewController, attribute);
            return Task.FromResult(true);
        }

        [Obsolete("", true)]
        protected virtual Task<bool> CloseRootViewController(ICrossViewModel viewModel, RootPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(viewModel);

            Logger?.LogWarning(
                "Ignored attempt to close the window root (ViewModel type: {ViewModelType}", viewModel.GetType().Name);

            return Task.FromResult(false);
        }

        [Obsolete]
        protected virtual Task<bool> CloseChildViewController(ICrossViewModel viewModel, ChildPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(viewModel);
            ArgumentNullException.ThrowIfNull(attribute);

#if IOS || MACCATALYST
            // if a popover is presented
            if (PopoverViewController is UINavigationController popoverNav &&
                TryCloseViewControllerInsideStack(popoverNav, viewModel, attribute))
            {
                return Task.FromResult(true);
            }
#endif

            // if there are modals presented
            if (ModalViewControllers.Count > 0 && CloseModalChildViewController(viewModel, attribute))
                return Task.FromResult(true);

            // if the current root is a TabBarViewController, delegate close responsibility to it
            if (Context.TabBarViewController?.CloseChildViewModel(viewModel) == true)
                return Task.FromResult(true);

            if (SplitViewController?.CloseChildViewModel(viewModel, attribute) == true)
                return Task.FromResult(true);

            // if the current root is a NavigationController, close it in the stack
            if (MasterNavigationController != null && TryCloseViewControllerInsideStack(MasterNavigationController, viewModel, attribute))
                return Task.FromResult(true);

            return Task.FromResult(false);
        }

        [Obsolete]
        private bool CloseModalChildViewController(ICrossViewModel viewModel, ChildPresentationAttribute attribute)
        {
            foreach (var modalNav in ModalViewControllers.OfType<UINavigationController>())
            {
                if (TryCloseViewControllerInsideStack(modalNav, viewModel, attribute))
                    return true;
            }

            return false;
        }

        [Obsolete("", true)]
        protected virtual Task<bool> CloseTabViewController(ICrossViewModel viewModel, TabPresentationAttribute attribute)
        {
            if (Context.TabBarViewController != null && Context.TabBarViewController.CloseTabViewModel(viewModel))
                return Task.FromResult(true);

            return Task.FromResult(false);
        }

        [Obsolete("", true)]
        protected virtual Task<bool> ClosePageViewController(ICrossViewModel viewModel, PagePresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(viewModel);
            ArgumentNullException.ThrowIfNull(attribute);

            if (PageViewController != null && PageViewController.RemovePage(viewModel))
                return Task.FromResult(true);

            return Task.FromResult(false);
        }

        [Obsolete("", true)]
        protected virtual Task<bool> CloseMasterSplitViewController(ICrossViewModel viewModel, SplitViewPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(viewModel);
            ArgumentNullException.ThrowIfNull(attribute);

            if (SplitViewController != null && SplitViewController.CloseChildViewModel(viewModel, attribute))
                return Task.FromResult(true);

            return Task.FromResult(true);
        }

        [Obsolete("", true)]
        protected virtual Task<bool> CloseDetailSplitViewController(ICrossViewModel viewModel, SplitViewPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(viewModel);
            ArgumentNullException.ThrowIfNull(attribute);

            if (SplitViewController != null && SplitViewController.CloseChildViewModel(viewModel, attribute))
                return Task.FromResult(true);

            return Task.FromResult(false);
        }

        [Obsolete("", true)]
        protected virtual Task<bool> CloseModalViewController(ICrossViewModel viewModel, ModalPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(viewModel);
            ArgumentNullException.ThrowIfNull(attribute);

            if (ModalViewControllers.Count == 0)
                return Task.FromResult(false);

            // check for plain modals
            var modalToClose =
                ModalViewControllers.Find(v => v is IMvxIosView iosView && iosView.ViewModel == viewModel);
            if (modalToClose != null)
            {
                return CloseModalViewController(modalToClose, attribute);
            }

            // check for modal navigation stacks
            UIViewController? controllerToClose = null;
            foreach (var vc in ModalViewControllers.OfType<UINavigationController>())
            {
                var root = vc.ViewControllers?.FirstOrDefault();
                if (root != null && root.GetIMvxIosView()?.ViewModel == viewModel)
                {
                    controllerToClose = vc;
                    break;
                }
            }

            if (controllerToClose != null)
            {
                return CloseModalViewController(controllerToClose, attribute);
            }

            return Task.FromResult(false);
        }

#if IOS || MACCATALYST
        protected virtual Task<bool> ClosePopoverViewController(ICrossViewModel viewModel, PopoverPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(viewModel);
            ArgumentNullException.ThrowIfNull(attribute);

            if (PopoverViewController == null)
                return Task.FromResult(false);

            // check for plain popover
            if (PopoverViewController is IMvxIosView iosView && iosView.ViewModel == viewModel)
            {
                return ClosePopoverViewController(PopoverViewController, attribute);
            }

            // check for popover navigation stack
            UIViewController? controllerToClose = null;
            if (PopoverViewController is UINavigationController vc)
            {
                var root = vc.ViewControllers?.FirstOrDefault();
                if (root is IMvxIosView rootIosView && rootIosView.ViewModel == viewModel)
                {
                    controllerToClose = vc;
                }
            }

            if (controllerToClose != null)
            {
                return ClosePopoverViewController(controllerToClose, attribute);
            }

            return Task.FromResult(false);
        }
#endif

        [Obsolete]
        protected virtual bool TryCloseViewControllerInsideStack(UINavigationController navController, ICrossViewModel toClose, ChildPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(navController);
            ArgumentNullException.ThrowIfNull(attribute);

            ArgumentNullException.ThrowIfNull(toClose);

            // check for top view controller
            var topView = navController.TopViewController;
            if (topView is IMvxIosView iosView && iosView.ViewModel == toClose)
            {
                navController.PopViewController(attribute.Animated);
                return true;
            }

            // loop through stack
            var controllers = navController.ViewControllers?.ToList();
            var controllerToClose = controllers?.Find(vc => vc is IMvxIosView iView && iView.ViewModel == toClose);
            if (controllerToClose != null)
            {
                controllers!.Remove(controllerToClose);
                navController.ViewControllers = controllers.ToArray();

                return true;
            }

            return false;
        }

        protected virtual NavigationController CreateNavigationController(UIViewController viewController)
        {
            ArgumentNullException.ThrowIfNull(viewController);

            return new NavigationController(viewController);
        }

        [Obsolete]
        protected virtual void PushViewControllerIntoStack(
            UINavigationController navigationController, UIViewController viewController, ChildPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(navigationController);
            ArgumentNullException.ThrowIfNull(viewController);
            ArgumentNullException.ThrowIfNull(attribute);

            navigationController.PushViewController(viewController, attribute.Animated);

            if (viewController is ITabBarViewController tabBarController)
                Context.TabBarViewController = tabBarController;
        }

        [Obsolete("", true)]
        protected virtual void CloseMasterNavigationController()
        {
            if (MasterNavigationController == null)
                return;

            if (MasterNavigationController.ViewControllers != null)
            {
                foreach (var item in MasterNavigationController.ViewControllers)
                    item.DidMoveToParentViewController(null);
            }

            MasterNavigationController = null;
        }

        [Obsolete("")]
        public virtual async Task<bool> CloseModalViewController(UIViewController viewController, ModalPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(viewController);
            ArgumentNullException.ThrowIfNull(attribute);

            if (viewController is UINavigationController modalNavController &&
                modalNavController.ViewControllers != null)
            {
                foreach (var item in modalNavController.ViewControllers)
                    item.DidMoveToParentViewController(null);
            }

            await viewController.DismissViewControllerAsync(attribute.Animated).ConfigureAwait(true);
            ModalViewControllers.Remove(viewController);
            return true;
        }

        [Obsolete("", true)]
        public virtual async Task<bool> CloseModalViewControllers()
        {
            while (ModalViewControllers.Count > 0)
            {
                var didClose =
                    await CloseModalViewController(ModalViewControllers[^1],
                        new ModalPresentationAttribute()).ConfigureAwait(true);

                if (!didClose)
                    return false;
            }

            return true;
        }

#if IOS || MACCATALYST
        public virtual async Task<bool> ClosePopoverViewController(UIViewController viewController, PopoverPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(viewController);
            ArgumentNullException.ThrowIfNull(attribute);

            if (viewController is UINavigationController { ViewControllers: not null } popoverNavController)
            {
                foreach (var item in popoverNavController.ViewControllers)
                    item.DidMoveToParentViewController(null);
            }

            await viewController.DismissViewControllerAsync(attribute.Animated).ConfigureAwait(true);
            PopoverViewController = null;
            return true;
        }
#endif

        [Obsolete("")]
        public virtual Task<bool> CloseTabBarViewController()
        {
            if (Context.TabBarViewController == null)
                return Task.FromResult(true);

            if (Context.TabBarViewController is UITabBarController tabsController
                && tabsController.ViewControllers != null)
            {
                foreach (var item in tabsController.ViewControllers)
                    item.DidMoveToParentViewController(null);
            }
            Context.TabBarViewController = null;
            return Task.FromResult(true);
        }

        [Obsolete("", true)]
        protected virtual Task<bool> CloseSplitViewController()
        {
            if (SplitViewController == null)
                return Task.FromResult(true);

            if (SplitViewController is UISplitViewController splitController)
            {
                foreach (var item in splitController.ViewControllers)
                    item.DidMoveToParentViewController(null);
            }
            SplitViewController = null;
            return Task.FromResult(true);
        }

#if IOS || MACCATALYST
        // Called if popover was dismissed by tapping outside view.
        public virtual void ClosedPopoverViewController()
        {
            PopoverViewController = null;
        }
#endif
    }
}