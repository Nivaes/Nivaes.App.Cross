using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public abstract class UIKitPressenterAction<TPressenterAttribute> : PressenterAction<TPressenterAttribute>
        where TPressenterAttribute : ICrossPresentationAttribute
    {
        protected readonly IMvxIosViewCreator ViewCreator;

        // ToDo: Ha de ser comun para todos los UIKitPressenterAction.
        protected UIWindow Window { get; }

        // ToDo: Ha de ser comun para todos los UIKitPressenterAction.
        protected UINavigationController? MasterNavigationController { get; set; }

        // ToDo: Ha de ser comun para todos los UIKitPressenterAction.
        protected IMvxTabBarViewController? TabBarViewController { get; set; }

        // ToDo: Ha de ser comun para todos los UIKitPressenterAction.
        protected List<UIViewController> ModalViewControllers { get; } = [];

        // ToDo: Ha de ser comun para todos los UIKitPressenterAction.
        protected IMvxSplitViewController? SplitViewController { get; set; }

        // ToDo: Ha de ser comun para todos los UIKitPressenterAction.
        protected IMvxPageViewController? PageViewController { get; set; }

#if IOS || MACCATALYST
        // ToDo: ¿Tiene que se común para todos los PressenterAction?
        public UIViewController? PopoverViewController { get; protected set; }
#endif

        #region Constructor
        public UIKitPressenterAction(
                ICrossViewsContainer viewsContainer,
                IMvxIosViewCreator viewCreator,
                ILogger logger)
            : base(viewsContainer, logger)
        {
            ViewCreator = viewCreator;
        }
        #endregion

        protected override CrossBasePresentationAttribute CreatePresentationAttribute(Type? viewModelType, Type? viewType)
        {
            ArgumentNullException.ThrowIfNull(viewModelType);
            ArgumentNullException.ThrowIfNull(viewType);

            if (MasterNavigationController == null &&
                TabBarViewController?.CanShowChildView() != true)
            {
                Logger?.LogTrace(
                    "PresentationAttribute nor MasterNavigationController found for {ViewTypeName}. Assuming Root presentation",
                    viewType?.Name);

                return new MvxRootPresentationAttribute
                {
                    WrapInNavigationController = true,
                    ViewType = viewType,
                    ViewModelType = viewModelType
                };
            }

            Logger?.LogTrace("PresentationAttribute not found for {ViewTypeName}. Assuming animated Child presentation", viewType?.Name);

            return new MvxChildPresentationAttribute { ViewType = viewType, ViewModelType = viewModelType };
        }

        protected ValueTask<bool> ShowRootViewController(
           UIViewController viewController,
           MvxRootPresentationAttribute attribute,
           CrossViewModelRequest request)
        {
            ArgumentNullException.ThrowIfNull(viewController);
            ArgumentNullException.ThrowIfNull(attribute);

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

        private async ValueTask<bool> ShowRootViewController(UIViewController viewController, MvxRootPresentationAttribute attribute)
        {
            SetupWindowRootNavigation(viewController, attribute);

            if (!await CloseModalViewControllers().ConfigureAwait(true)) return false;
            if (!await CloseTabBarViewController().ConfigureAwait(true)) return false;
            if (!await CloseSplitViewController().ConfigureAwait(true)) return false;
            return true;
        }

        private async ValueTask<bool> ShowSplitRootViewController(UIViewController viewController, MvxRootPresentationAttribute attribute,
            IMvxSplitViewController splitController)
        {
            SplitViewController = splitController;

            // set root
            SetupWindowRootNavigation(viewController, attribute);

            if (!await CloseModalViewControllers().ConfigureAwait(true)) return false;
            if (!await CloseTabBarViewController().ConfigureAwait(true)) return false;

            return true;
        }

        private async ValueTask<bool> ShowPageRootViewController(UIViewController viewController, MvxRootPresentationAttribute attribute,
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
            UIViewController viewController, MvxRootPresentationAttribute attribute,
            IMvxTabBarViewController tabBarController)
        {
            TabBarViewController = tabBarController;

            // set root
            SetupWindowRootNavigation(viewController, attribute);

            if (!await CloseModalViewControllers().ConfigureAwait(true)) return false;
            if (!await CloseSplitViewController().ConfigureAwait(true)) return false;

            return true;
        }

        protected void SetupWindowRootNavigation(UIViewController viewController, MvxRootPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(viewController);
            ArgumentNullException.ThrowIfNull(attribute);

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
           MvxSplitViewPresentationAttribute attribute,
           CrossViewModelRequest request)
        {
            ArgumentNullException.ThrowIfNull(viewController);
            ArgumentNullException.ThrowIfNull(attribute);

            if (SplitViewController == null)
                throw new CrossException("Trying to show a master page without a SplitViewController, this is not possible!");

            SplitViewController.ShowMasterView(viewController, attribute);
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

        public virtual async ValueTask<bool> CloseModalViewController(UIViewController viewController, MvxModalPresentationAttribute attribute)
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
                        new MvxModalPresentationAttribute()).ConfigureAwait(true);

                if (!didClose)
                    return false;
            }

            return true;
        }

        public virtual Task<bool> CloseTabBarViewController()
        {
            if (TabBarViewController == null)
                return Task.FromResult(true);

            if (TabBarViewController is UITabBarController tabsController
                && tabsController.ViewControllers != null)
            {
                foreach (var item in tabsController.ViewControllers)
                    item.DidMoveToParentViewController(null);
            }
            TabBarViewController = null;
            return Task.FromResult(true);
        }

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

        protected MvxNavigationController CreateNavigationController(UIViewController viewController)
        {
            return new MvxNavigationController(viewController);
        }

        protected UIViewController GetParentViewController()
        {
            //Ensure to get a ViewController that is not being dismissed. See related bugs https://github.com/MvvmCross/MvvmCross/issues/4781
            return ModalViewControllers.LastOrDefault(x => !x.IsBeingDismissed) ?? Window.RootViewController
                ?? throw new CrossException($"No parent ViewController found.");
        }

        protected void SetWindowRootViewController(UIViewController controller, MvxRootPresentationAttribute? attribute = null)
        {
            RemoveWindowSubviews();

            if (attribute == null || attribute.AnimationOptions == UIViewAnimationOptions.TransitionNone)
            {
                Window.RootViewController = controller;
                return;
            }

            UIView.Transition(
                Window, attribute.AnimationDuration, attribute.AnimationOptions,
                () => Window.RootViewController = controller, null
            );
        }

        protected void RemoveWindowSubviews()
        {
            foreach (var v in Window.Subviews)
                v.RemoveFromSuperview();
        }

        protected ValueTask<bool> ShowChildViewController(
           UIViewController viewController,
           MvxChildPresentationAttribute attribute,
           CrossViewModelRequest request)
        {
            ArgumentNullException.ThrowIfNull(viewController);
            ArgumentNullException.ThrowIfNull(attribute);

            if (viewController is IMvxSplitViewController)
                throw new CrossException("A SplitViewController cannot be presented as a child. Consider using Root instead");

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

            if (TabBarViewController != null && TabBarViewController.ShowChildView(viewController))
            {
                return ValueTask.FromResult(true);
            }

            if (MasterNavigationController != null)
            {
                PushViewControllerIntoStack(MasterNavigationController, viewController, attribute);
                return ValueTask.FromResult(true);
            }

            throw new CrossException($"Trying to show View type: {viewController.GetType().Name} as child, but there is no current stack!");
        }

        private ValueTask<bool> ShowModalViewControllerChild(UIViewController viewController, MvxChildPresentationAttribute attribute)
        {
            if (ModalViewControllers.LastOrDefault() is UINavigationController modalNavController)
            {
                PushViewControllerIntoStack(modalNavController, viewController, attribute);

                return ValueTask.FromResult(true);
            }

            throw new CrossException(
                $"Trying to show View type: {viewController.GetType().Name} as child, but there is currently a plain modal view presented!");
        }

#if IOS || MACCATALYST
        private ValueTask<bool> ShowPopoverViewControllerChild(UIViewController viewController, MvxChildPresentationAttribute attribute)
        {
            if (PopoverViewController is UINavigationController popoverNavController)
            {
                PushViewControllerIntoStack(popoverNavController, viewController, attribute);

                return ValueTask.FromResult(true);
            }

            throw new CrossException(
                $"Trying to show View type: {viewController.GetType().Name} as child, but there is currently a plain popover view presented!");
        }
#endif

        protected virtual void PushViewControllerIntoStack(
            UINavigationController navigationController, UIViewController viewController, MvxChildPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(navigationController);
            ArgumentNullException.ThrowIfNull(viewController);
            ArgumentNullException.ThrowIfNull(attribute);

            navigationController.PushViewController(viewController, attribute.Animated);

            if (viewController is IMvxTabBarViewController tabBarController)
                TabBarViewController = tabBarController;
        }

    }
}
