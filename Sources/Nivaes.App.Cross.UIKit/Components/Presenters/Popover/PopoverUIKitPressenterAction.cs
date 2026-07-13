#if IOS || MACCATALYST
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public abstract class PopoverUIKitPressenterAction
            : UIKitPressenterAction<MvxPopoverPresentationAttribute>
    {
        #region Constructor
        public PopoverUIKitPressenterAction(
                ICrossViewsContainer viewsContainer,
                IMvxIosViewCreator viewCreator,
                ILogger<SplitUIKitPressenterAction> logger)
            : base(viewsContainer, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(Type viewType, MvxPopoverPresentationAttribute attribute, CrossViewModelRequest request)
        {
            var viewController = (UIViewController?)ViewCreator.CreateView(request);
            if (viewController == null)
            {
                Logger?.LogWarning(
                    "Got null ViewController for request {Request}", request);

                return ValueTask.FromResult(false);
            }
            return ShowPopoverViewController(viewController, attribute, request);
        }

        protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, MvxPopoverPresentationAttribute attribute)
        {
            ArgumentNullException.ThrowIfNull(viewModel);
            ArgumentNullException.ThrowIfNull(attribute);

            if (PopoverViewController == null)
                return ValueTask.FromResult(false);

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

            return ValueTask.FromResult(false);
        }

        private async ValueTask<bool> ShowPopoverViewController(
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
                presentationController.Delegate = new PopoverPresentationControllerDelegate(this);
            }

            PopoverViewController = viewController;

            var parentViewController = GetParentViewController();
            await parentViewController.PresentViewControllerAsync(viewController, attribute.Animated).ConfigureAwait(true);
            return true;
        }

        public virtual async ValueTask<bool> ClosePopoverViewController(UIViewController viewController, MvxPopoverPresentationAttribute attribute)
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

        // Called if popover was dismissed by tapping outside view.
        public void ClosedPopoverViewController()
        {
            PopoverViewController = null;
        }
    }
}
#endif