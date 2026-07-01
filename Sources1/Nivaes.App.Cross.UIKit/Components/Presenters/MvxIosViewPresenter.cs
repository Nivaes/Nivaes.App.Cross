using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MvvmCross.Platforms.Ios.Presenters;
using MvvmCross.Platforms.Ios.Presenters.Attributes;
using MvvmCross.Platforms.Ios.Views;

namespace Nivaes.App.Cross.UIKitOS
{
    public class MvxIosViewPresenter
        : CrossAttributeViewPresenter, IMvxIosViewPresenter
    {
        //private readonly MvxIosMajorVersionChecker _iosVersion13Checker = new(13);
        private readonly IMvxIosViewCreator _viewCreator;

        protected UIWindow Window { get; }

        public UINavigationController? MasterNavigationController { get; protected set; }

#if IOS || MACCATALYST
        public UIViewController? PopoverViewController { get; protected set; }
#endif

        public List<UIViewController> ModalViewControllers { get; } = [];

        public IMvxTabBarViewController? TabBarViewController { get; protected set; }

        public IMvxPageViewController? PageViewController { get; protected set; }

        public IMvxSplitViewController? SplitViewController { get; protected set; }

        public MvxIosViewPresenter(UIWindow window, ICrossViewsContainer crossViewsContainer, IMvxIosViewCreator viewCreator,
            ILogger<MvxIosViewPresenter> logger)
            : base(crossViewsContainer, logger)
        {
            _viewCreator = viewCreator;
            Window = window;
        }

        public override CrossBasePresentationAttribute CreatePresentationAttribute(
                [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type? viewModelType,
                [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type? viewType)
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

            Logger?.LogTrace(
                "PresentationAttribute not found for {ViewTypeName}. Assuming animated Child presentation", viewType?.Name);

            return new MvxChildPresentationAttribute { ViewType = viewType, ViewModelType = viewModelType };
        }

        public override object? CreateOverridePresentationAttributeViewInstance(
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] Type viewType)
        {
            ArgumentNullException.ThrowIfNull(viewType);

            return (UIViewController?)_viewCreator.CreateViewOfType(viewType);
        }

        public override void RegisterAttributeTypes()
        {
            if (AttributeTypesToActionsDictionary == null)
                throw new InvalidOperationException("Cannot register attribute types on null dictionary");

            AttributeTypesToActionsDictionary.Register<MvxRootPresentationAttribute>(
                (_, attribute, request) =>
                {
                    var viewController = (UIViewController?)_viewCreator.CreateView(request);
                    if (viewController == null)
                    {
                        Logger.LogWarning(
                            "Got null ViewController for request {Request}", request);

                        return Task.FromResult(false);
                    }
                    return ShowRootViewController(viewController, attribute, request);
                },
                CloseRootViewController);

            AttributeTypesToActionsDictionary.Register<MvxChildPresentationAttribute>(
                (_, attribute, request) =>
                {
                    var viewController = (UIViewController?)_viewCreator.CreateView(request);
                    if (viewController == null)
                    {
                        Logger.LogWarning(
                            "Got null ViewController for request {Request}", request);

                        return Task.FromResult(false);
                    }
                    return ShowChildViewController(viewController, attribute, request);
                },
                CloseChildViewController);

            AttributeTypesToActionsDictionary.Register<MvxTabPresentationAttribute>(
                (_, attribute, request) =>
                {
                    var viewController = (UIViewController?)_viewCreator.CreateView(request);
                    if (viewController == null)
                    {
                        Logger?.LogWarning(
                            "Got null ViewController for request {Request}", request);

                        return Task.FromResult(false);
                    }
                    return ShowTabViewController(viewController, attribute, request);
                },
                CloseTabViewController);

            AttributeTypesToActionsDictionary.Register<MvxPagePresentationAttribute>(
                (_, attribute, request) =>
                {
                    var viewController = (UIViewController?)_viewCreator.CreateView(request);
                    if (viewController == null)
                    {
                        Logger?.LogWarning(
                            "Got null ViewController for request {Request}", request);

                        return Task.FromResult(false);
                    }
                    return ShowPageViewController(viewController, attribute, request);
                },
                ClosePageViewController);

            AttributeTypesToActionsDictionary.Register<MvxModalPresentationAttribute>(
                (_, attribute, request) =>
                {
                    var viewController = (UIViewController?)_viewCreator.CreateView(request);
                    if (viewController == null)
                    {
                        Logger?.LogWarning(
                            "Got null ViewController for request {Request}", request);

                        return Task.FromResult(false);
                    }
                    return ShowModalViewController(viewController, attribute, request);
                },
                CloseModalViewController);

            AttributeTypesToActionsDictionary.Register<MvxSplitViewPresentationAttribute>(
                (_, attribute, request) =>
                {
                    var viewController = (UIViewController?)_viewCreator.CreateView(request);
                    if (viewController == null)
                    {
                        Logger?.LogWarning(
                            "Got null ViewController for request {Request}", request);

                        return Task.FromResult(false);
                    }

                    var splitAttribute = attribute;
                    return splitAttribute.Position switch
                    {
                        MasterDetailPosition.Master =>
                            ShowMasterSplitViewController(viewController, splitAttribute, request),
                        MasterDetailPosition.Detail =>
                            ShowDetailSplitViewController(viewController, splitAttribute, request),
                        _ => Task.FromResult(true)
                    };
                },
                (viewModel, attribute) =>
                {
                    var splitAttribute = attribute;
                    return splitAttribute.Position switch
                    {
                        MasterDetailPosition.Master => CloseMasterSplitViewController(viewModel, splitAttribute),
                        MasterDetailPosition.Detail => CloseDetailSplitViewController(viewModel, splitAttribute),
                        _ => CloseDetailSplitViewController(viewModel, splitAttribute)
                    };
                });

#if IOS || MACCATALYST
            RegisterPopoverAttributeType();
#endif
        }

#if IOS || MACCATALYST
        protected virtual void RegisterPopoverAttributeType()
        {
            AttributeTypesToActionsDictionary.Register<MvxPopoverPresentationAttribute>(
                (_, attribute, request) =>
                {
                    var viewController = (UIViewController?)_viewCreator.CreateView(request);
                    if (viewController == null)
                    {
                        Logger?.LogWarning(
                            "Got null ViewController for request {Request}", request);

                        return Task.FromResult(false);
                    }
                    return ShowPopoverViewController(viewController, attribute, request);
                },
                ClosePopoverViewController);
        }
#endif

        protected virtual Task<bool> ShowRootViewController(
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

        private async Task<bool> ShowRootViewController(UIViewController viewController, MvxRootPresentationAttribute attribute)
        {
            SetupWindowRootNavigation(viewController, attribute);

            if (!await CloseModalViewControllers().ConfigureAwait(true)) return false;
            if (!await CloseTabBarViewController().ConfigureAwait(true)) return false;
            if (!await CloseSplitViewController().ConfigureAwait(true)) return false;
            return true;
        }

        private async Task<bool> ShowSplitRootViewController(UIViewController viewController, MvxRootPresentationAttribute attribute,
            IMvxSplitViewController splitController)
        {
            SplitViewController = splitController;

            // set root
            SetupWindowRootNavigation(viewController, attribute);

            if (!await CloseModalViewControllers().ConfigureAwait(true)) return false;
            if (!await CloseTabBarViewController().ConfigureAwait(true)) return false;

            return true;
        }

        private async Task<bool> ShowPageRootViewController(UIViewController viewController, MvxRootPresentationAttribute attribute,
            IMvxPageViewController pageViewController)
        {
            PageViewController = pageViewController;

            // set root
            SetupWindowRootNavigation(viewController, attribute);

            if (!await CloseModalViewControllers().ConfigureAwait(true)) return false;
            if (!await CloseSplitViewController().ConfigureAwait(true)) return false;

            return true;
        }

        private async Task<bool> ShowTabBarRootViewController(
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

        public override Task<bool> ChangePresentation(CrossPresentationHint hint)
        {
            ArgumentNullException.ThrowIfNull(hint);

            return hint switch
            {
                CrossPagePresentationHint pagePresentationHint when ChangePagePresentation(pagePresentationHint) =>
                    Task.FromResult(true),
                _ => base.ChangePresentation(hint)
            };
        }

        private bool ChangePagePresentation(CrossPagePresentationHint pagePresentationHint)
        {
            if (!(TabBarViewController is UITabBarController tabsController) ||
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

        protected virtual Task<bool> ShowChildViewController(
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
                return Task.FromResult(true);
            }

            if (MasterNavigationController != null)
            {
                PushViewControllerIntoStack(MasterNavigationController, viewController, attribute);
                return Task.FromResult(true);
            }

            throw new CrossException($"Trying to show View type: {viewController.GetType().Name} as child, but there is no current stack!");
        }

        private Task<bool> ShowModalViewControllerChild(UIViewController viewController, MvxChildPresentationAttribute attribute)
        {
            if (ModalViewControllers.LastOrDefault() is UINavigationController modalNavController)
            {
                PushViewControllerIntoStack(modalNavController, viewController, attribute);

                return Task.FromResult(true);
            }

            throw new CrossException(
                $"Trying to show View type: {viewController.GetType().Name} as child, but there is currently a plain modal view presented!");
        }

#if IOS || MACCATALYST
        private Task<bool> ShowPopoverViewControllerChild(UIViewController viewController, MvxChildPresentationAttribute attribute)
        {
            if (PopoverViewController is UINavigationController popoverNavController)
            {
                PushViewControllerIntoStack(popoverNavController, viewController, attribute);

                return Task.FromResult(true);
            }

            throw new CrossException(
                $"Trying to show View type: {viewController.GetType().Name} as child, but there is currently a plain popover view presented!");
        }
#endif

        protected virtual Task<bool> ShowTabViewController(
            UIViewController viewController,
            MvxTabPresentationAttribute attribute,
            CrossViewModelRequest request)
        {
            ArgumentNullException.ThrowIfNull(viewController);
            ArgumentNullException.ThrowIfNull(attribute);

            if (TabBarViewController == null)
                throw new CrossException("Trying to show a tab without a TabBarViewController, this is not possible!");

            if (viewController is IMvxTabBarItemViewController tabBarItem)
            {
                attribute.TabName = tabBarItem.TabName;
                attribute.TabIconName = tabBarItem.TabIconName;
                attribute.TabSelectedIconName = tabBarItem.TabSelectedIconName;
            }

            if (attribute.WrapInNavigationController)
                viewController = CreateNavigationController(viewController);

            TabBarViewController.ShowTabView(
                viewController,
                attribute);
            return Task.FromResult(true);
        }

        protected virtual Task<bool> ShowPageViewController(
            UIViewController viewController,
            MvxPagePresentationAttribute attribute,
            CrossViewModelRequest request)
        {
            ArgumentNullException.ThrowIfNull(viewController);
            ArgumentNullException.ThrowIfNull(attribute);

            if (PageViewController == null)
                throw new CrossException("Trying to show a page without a PageViewController, this is not possible!");

            if (attribute.WrapInNavigationController)
                viewController = CreateNavigationController(viewController);

            PageViewController.AddPage(
                viewController,
                attribute);
            return Task.FromResult(true);
        }

        protected virtual IUIAdaptivePresentationControllerDelegate CreateModalPresentationControllerDelegate(
            UIViewController viewController, MvxModalPresentationAttribute attribute)
        {
            return new MvxModalPresentationControllerDelegate(this, viewController, attribute);
        }

        protected virtual Task<bool> ShowModalViewController(
            UIViewController viewController,
            MvxModalPresentationAttribute attribute,
            CrossViewModelRequest? request)
        {
            ArgumentNullException.ThrowIfNull(viewController);
            ArgumentNullException.ThrowIfNull(attribute);

            // setup modal based on attribute
            if (attribute.WrapInNavigationController)
            {
                viewController = CreateNavigationController(viewController);
            }

            viewController.ModalPresentationStyle = attribute.ModalPresentationStyle;
            viewController.ModalTransitionStyle = attribute.ModalTransitionStyle;
            if (attribute.PreferredContentSize != default)
                viewController.PreferredContentSize = attribute.PreferredContentSize;

            if (/*_iosVersion13Checker.IsVersionOrHigher &&*/ viewController.PresentationController != null)
            {
                viewController.PresentationController.Delegate =
                    CreateModalPresentationControllerDelegate(viewController, attribute);
            }

            var parentViewController = GetParentViewController();
            parentViewController.PresentViewController(viewController, attribute.Animated, null);

            ModalViewControllers.Add(viewController);

            return Task.FromResult(true);
        }

#if IOS || MACCATALYST
        protected virtual async Task<bool> ShowPopoverViewController(
            UIViewController viewController,
            MvxPopoverPresentationAttribute attribute,
            CrossViewModelRequest request)
        {
            ArgumentNullException.ThrowIfNull(viewController);
            ArgumentNullException.ThrowIfNull(attribute);

            if (PopoverViewController != null)
                throw new CrossException($"Trying to show View type: {viewController.GetType().Name} as popover, but there is already a popover present!");

            // Content size should be set to a target view controller, not the navigation one
            if (attribute.PreferredContentSize != default)
            {
                viewController.PreferredContentSize = attribute.PreferredContentSize;
            }

            // setup popover based on attribute
            if (attribute.WrapInNavigationController)
            {
                viewController = CreateNavigationController(viewController);
            }

            viewController.ModalPresentationStyle = UIModalPresentationStyle.Popover;

            var presentationController = viewController.PopoverPresentationController;
            if (presentationController != null)
            {
                presentationController.PermittedArrowDirections = attribute.PermittedArrowDirections;
                var sourceProvider = IPlatformApplication.Current!.Services.GetRequiredService<IMvxPopoverPresentationSourceProvider>();
                sourceProvider?.SetSource(presentationController);
                presentationController.Delegate = new MvxPopoverPresentationControllerDelegate(this);
            }

            PopoverViewController = viewController;

            var parentViewController = GetParentViewController();
            await parentViewController.PresentViewControllerAsync(viewController, attribute.Animated).ConfigureAwait(true);
            return true;
        }
#endif

        private UIViewController GetParentViewController()
        {
            //Ensure to get a ViewController that is not being dismissed. See related bugs https://github.com/MvvmCross/MvvmCross/issues/4781
            return ModalViewControllers.LastOrDefault(x => !x.IsBeingDismissed) ?? Window.RootViewController
                ?? throw new CrossException($"No parent ViewController found.");
        }

        protected virtual Task<bool> ShowMasterSplitViewController(
            UIViewController viewController,
            MvxSplitViewPresentationAttribute attribute,
            CrossViewModelRequest request)
        {
            ArgumentNullException.ThrowIfNull(viewController);
            ArgumentNullException.ThrowIfNull(attribute);

            if (SplitViewController == null)
                throw new CrossException("Trying to show a master page without a SplitViewController, this is not possible!");

            SplitViewController.ShowMasterView(viewController, attribute);
            return Task.FromResult(true);
        }

        protected virtual Task<bool> ShowDetailSplitViewController(
            UIViewController viewController,
            MvxSplitViewPresentationAttribute attribute,
            CrossViewModelRequest request)
        {
            ArgumentNullException.ThrowIfNull(viewController);
            ArgumentNullException.ThrowIfNull(attribute);

            if (SplitViewController == null)
                throw new CrossException("Trying to show a detail page without a SplitViewController, this is not possible!");

            SplitViewController.ShowDetailView(viewController, attribute);
            return Task.FromResult(true);
        }

        protected virtual Task<bool> CloseRootViewController(ICrossViewModel viewModel, MvxRootPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(viewModel);

            Logger?.LogWarning(
                "Ignored attempt to close the window root (ViewModel type: {ViewModelType}", viewModel.GetType().Name);

            return Task.FromResult(false);
        }

        protected virtual Task<bool> CloseChildViewController(ICrossViewModel viewModel, MvxChildPresentationAttribute attribute)
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
            if (TabBarViewController?.CloseChildViewModel(viewModel) == true)
                return Task.FromResult(true);

            if (SplitViewController?.CloseChildViewModel(viewModel, attribute) == true)
                return Task.FromResult(true);

            // if the current root is a NavigationController, close it in the stack
            if (MasterNavigationController != null && TryCloseViewControllerInsideStack(MasterNavigationController, viewModel, attribute))
                return Task.FromResult(true);

            return Task.FromResult(false);
        }

        private bool CloseModalChildViewController(ICrossViewModel viewModel, MvxChildPresentationAttribute attribute)
        {
            foreach (var modalNav in ModalViewControllers.OfType<UINavigationController>())
            {
                if (TryCloseViewControllerInsideStack(modalNav, viewModel, attribute))
                    return true;
            }

            return false;
        }

        protected virtual Task<bool> CloseTabViewController(ICrossViewModel viewModel, MvxTabPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(viewModel);
            ArgumentNullException.ThrowIfNull(attribute);

            if (TabBarViewController != null && TabBarViewController.CloseTabViewModel(viewModel))
                return Task.FromResult(true);

            return Task.FromResult(false);
        }

        protected virtual Task<bool> ClosePageViewController(ICrossViewModel viewModel, MvxPagePresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(viewModel);
            ArgumentNullException.ThrowIfNull(attribute);

            if (PageViewController != null && PageViewController.RemovePage(viewModel))
                return Task.FromResult(true);

            return Task.FromResult(false);
        }

        protected virtual Task<bool> CloseMasterSplitViewController(ICrossViewModel viewModel, MvxSplitViewPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(viewModel);
            ArgumentNullException.ThrowIfNull(attribute);

            if (SplitViewController != null && SplitViewController.CloseChildViewModel(viewModel, attribute))
                return Task.FromResult(true);

            return Task.FromResult(true);
        }

        protected virtual Task<bool> CloseDetailSplitViewController(ICrossViewModel viewModel, MvxSplitViewPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(viewModel);
            ArgumentNullException.ThrowIfNull(attribute);

            if (SplitViewController != null && SplitViewController.CloseChildViewModel(viewModel, attribute))
                return Task.FromResult(true);

            return Task.FromResult(false);
        }

        protected virtual Task<bool> CloseModalViewController(ICrossViewModel viewModel, MvxModalPresentationAttribute attribute)
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
        protected virtual Task<bool> ClosePopoverViewController(ICrossViewModel viewModel, MvxPopoverPresentationAttribute attribute)
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

        protected virtual bool TryCloseViewControllerInsideStack(UINavigationController navController, ICrossViewModel toClose, MvxChildPresentationAttribute attribute)
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

        protected virtual MvxNavigationController CreateNavigationController(UIViewController viewController)
        {
            ArgumentNullException.ThrowIfNull(viewController);

            return new MvxNavigationController(viewController);
        }

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

        public virtual async Task<bool> CloseModalViewController(UIViewController viewController, MvxModalPresentationAttribute attribute)
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

#if IOS || MACCATALYST
        public virtual async Task<bool> ClosePopoverViewController(UIViewController viewController, MvxPopoverPresentationAttribute attribute)
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

        protected void RemoveWindowSubviews()
        {
            foreach (var v in Window.Subviews)
                v.RemoveFromSuperview();
        }

        public virtual Task<bool> ShowModalViewController(UIViewController viewController, bool animated)
        {
            return ShowModalViewController(viewController, new MvxModalPresentationAttribute { Animated = animated }, null);
        }

        protected virtual void SetWindowRootViewController(UIViewController controller, MvxRootPresentationAttribute? attribute = null)
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

#if IOS || MACCATALYST
        // Called if popover was dismissed by tapping outside view.
        public virtual void ClosedPopoverViewController()
        {
            PopoverViewController = null;
        }
#endif
    }
}