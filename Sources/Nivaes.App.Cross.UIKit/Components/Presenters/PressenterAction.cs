using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public abstract class PressenterAction<TPressenterAttribute> 
        : Cross.PressenterAction<TPressenterAttribute>
        where TPressenterAttribute : IPresentationAttribute
    {
        protected readonly IMvxIosViewCreator ViewCreator;

        protected readonly PressenterActionContext Context;

        // ToDo: Ha de ser comun para todos los UIKitPressenterAction.
        protected UINavigationController? MasterNavigationController { get; set; }

        // ToDo: Ha de ser comun para todos los UIKitPressenterAction.
        //protected IMvxTabBarViewController? TabBarViewController { get; set; }

        // ToDo: Ha de ser comun para todos los UIKitPressenterAction.
        protected List<UIViewController> ModalViewControllers { get; } = [];

        // ToDo: Ha de ser comun para todos los UIKitPressenterAction.
        //protected IMvxSplitViewController? SplitViewController { get; set; }

        // ToDo: Ha de ser comun para todos los UIKitPressenterAction.
        protected IMvxPageViewController? PageViewController { get; set; }

#if IOS || MACCATALYST
        // ToDo: ¿Tiene que se común para todos los PressenterAction?
        public UIViewController? PopoverViewController { get; protected set; }
#endif

        #region Constructor
        public PressenterAction(
                PressenterActionContext context,
                ICrossViewsContainer viewsContainer,
                IMvxIosViewCreator viewCreator,
                ILogger logger)
            : base(viewsContainer, logger)
        {
            Context = context;
            ViewCreator = viewCreator;
        }
        #endregion

        protected override BasePresentationAttribute CreatePresentationAttribute(Type? viewModelType, Type? viewType)
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

            Logger?.LogTrace("PresentationAttribute not found for {ViewTypeName}. Assuming animated Child presentation", viewType?.Name);

            return new ChildPresentationAttribute { ViewType = viewType, ViewModelType = viewModelType };
        }

        protected ValueTask<bool> ShowRootViewController(
           UIViewController viewController,
           RootPresentationAttribute attribute,
           CrossViewModelRequest request)
        {
            return viewController switch
            {
                // check if viewController is a TabBarController
                IMvxTabBarViewController tabBarController =>
                    ShowTabBarRootViewController(viewController, attribute, tabBarController),
                // check if viewController is a PageViewController
                IMvxPageViewController pageViewController =>
                    ShowPageRootViewController(viewController, attribute, pageViewController),
                // check if viewController is a SplitViewController
                IMvxSplitViewController splitController =>
                    ShowSplitRootViewController(viewController, attribute, splitController),
                // set root initiating stack navigation or just a plain controller
                _ => ShowRootViewController(viewController, attribute)
            };
        }

        private async ValueTask<bool> ShowRootViewController(UIViewController viewController, RootPresentationAttribute attribute)
        {
            SetupWindowRootNavigation(viewController, attribute);

            if (!await CloseModalViewControllers().ConfigureAwait(true)) return false;
            if (!await CloseTabBarViewController().ConfigureAwait(true)) return false;
            if (!await CloseSplitViewController().ConfigureAwait(true)) return false;
            return true;
        }

        private async ValueTask<bool> ShowSplitRootViewController(UIViewController viewController, RootPresentationAttribute attribute,
            IMvxSplitViewController splitController)
        {
            Context.SplitViewController = splitController;

            // set root
            SetupWindowRootNavigation(viewController, attribute);

            if (!await CloseModalViewControllers().ConfigureAwait(true)) return false;
            if (!await CloseTabBarViewController().ConfigureAwait(true)) return false;

            return true;
        }

        private async ValueTask<bool> ShowPageRootViewController(UIViewController viewController, RootPresentationAttribute attribute,
            IMvxPageViewController pageViewController)
        {
            PageViewController = pageViewController;

            // set root
            SetupWindowRootNavigation(viewController, attribute);

            if (!await CloseModalViewControllers().ConfigureAwait(true)) return false;
            if (!await CloseSplitViewController().ConfigureAwait(true)) return false;

            return true;
        }

        private async ValueTask<bool> ShowTabBarRootViewController(
            UIViewController viewController, RootPresentationAttribute attribute,
            IMvxTabBarViewController tabBarController)
        {
            Context.TabBarViewController = tabBarController;

            // set root
            SetupWindowRootNavigation(viewController, attribute);

            if (!await CloseModalViewControllers().ConfigureAwait(true)) return false;
            if (!await CloseSplitViewController().ConfigureAwait(true)) return false;

            return true;
        }

        protected void SetupWindowRootNavigation(UIViewController viewController, RootPresentationAttribute attribute)
        {
            if (attribute.WrapInNavigationController)
            {
                MasterNavigationController = CreateNavigationController(viewController);

                SetWindowRootViewController(MasterNavigationController, attribute);
            }
            else
            {
                SetWindowRootViewController(viewController, attribute);

                CloseMasterNavigationController();
            }
        }

        protected ValueTask<bool> ShowMasterSplitViewController(
           UIViewController viewController,
           SplitViewPresentationAttribute attribute,
           CrossViewModelRequest request)
        {
            if (Context.SplitViewController == null)
                throw new AppException("Trying to show a master page without a SplitViewController, this is not possible!");

            Context.SplitViewController.ShowMasterView(viewController, attribute);
            return ValueTask.FromResult(true);
        }

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

        public virtual async ValueTask<bool> CloseModalViewController(UIViewController viewController, ModalPresentationAttribute attribute)
        {
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

        protected virtual Task<bool> CloseSplitViewController()
        {
            if (Context.SplitViewController == null)
                return Task.FromResult(true);

            if (Context.SplitViewController is UISplitViewController splitController)
            {
                foreach (var item in splitController.ViewControllers)
                    item.DidMoveToParentViewController(null);
            }
            Context.SplitViewController = null;
            return Task.FromResult(true);
        }

        protected MvxNavigationController CreateNavigationController(UIViewController viewController)
        {
            return new MvxNavigationController(viewController);
        }

        protected UIViewController GetParentViewController()
        {
            //Ensure to get a ViewController that is not being dismissed. See related bugs https://github.com/MvvmCross/MvvmCross/issues/4781
            return ModalViewControllers.LastOrDefault(x => !x.IsBeingDismissed) ?? Context.Window.RootViewController
                ?? throw new AppException($"No parent ViewController found.");
        }

        protected void SetWindowRootViewController(UIViewController controller, RootPresentationAttribute? attribute = null)
        {
            RemoveWindowSubviews();

            if (attribute == null || attribute.AnimationOptions == UIViewAnimationOptions.TransitionNone)
            {
                Context.Window.RootViewController = controller;
                return;
            }

            UIView.Transition(
                Context.Window, attribute.AnimationDuration, attribute.AnimationOptions,
                () => Context.Window.RootViewController = controller, null
            );
        }

        protected void RemoveWindowSubviews()
        {
            foreach (var v in Context.Window.Subviews)
                v.RemoveFromSuperview();
        }

        protected ValueTask<bool> ShowChildViewController(
           UIViewController viewController,
           ChildPresentationAttribute attribute,
           CrossViewModelRequest request)
        {
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
                return ValueTask.FromResult(true);
            }

            if (MasterNavigationController != null)
            {
                PushViewControllerIntoStack(MasterNavigationController, viewController, attribute);
                return ValueTask.FromResult(true);
            }

            throw new AppException($"Trying to show View type: {viewController.GetType().Name} as child, but there is no current stack!");
        }

        private ValueTask<bool> ShowModalViewControllerChild(UIViewController viewController, ChildPresentationAttribute attribute)
        {
            if (ModalViewControllers.LastOrDefault() is UINavigationController modalNavController)
            {
                PushViewControllerIntoStack(modalNavController, viewController, attribute);

                return ValueTask.FromResult(true);
            }

            throw new AppException(
                $"Trying to show View type: {viewController.GetType().Name} as child, but there is currently a plain modal view presented!");
        }

#if IOS || MACCATALYST
        private ValueTask<bool> ShowPopoverViewControllerChild(UIViewController viewController, ChildPresentationAttribute attribute)
        {
            if (PopoverViewController is UINavigationController popoverNavController)
            {
                PushViewControllerIntoStack(popoverNavController, viewController, attribute);

                return ValueTask.FromResult(true);
            }

            throw new AppException(
                $"Trying to show View type: {viewController.GetType().Name} as child, but there is currently a plain popover view presented!");
        }
#endif

        protected virtual void PushViewControllerIntoStack(
            UINavigationController navigationController, UIViewController viewController, ChildPresentationAttribute attribute)
        {
            navigationController.PushViewController(viewController, attribute.Animated);

            if (viewController is IMvxTabBarViewController tabBarController)
                Context.TabBarViewController = tabBarController;
        }
    }
}
