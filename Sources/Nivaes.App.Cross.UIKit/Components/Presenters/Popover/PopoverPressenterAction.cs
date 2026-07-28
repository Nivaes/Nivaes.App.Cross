#if IOS || MACCATALYST
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public sealed class PopoverPressenterAction
            : PressenterAction<PopoverPresentationAttribute>
    {
        #region Constructor
        public PopoverPressenterAction(
                IPressenterActionContext context,
                IMvxIosViewCreator viewCreator,
                ILogger<SplitPressenterAction> logger)
            : base(context, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(IViewModelRequest request, PopoverPresentationAttribute attribute)
        {
            var viewController = (UIViewController?)ViewCreator.CreateView(request);
            if (viewController == null)
            {
                Logger?.LogWarning("Got null ViewController for request {Request}", request);

                return ValueTask.FromResult(false);
            }
            return ShowPopoverViewController(viewController, attribute, request);
        }

        protected override ValueTask<bool> CloseAction(IViewModelRequest request, PopoverPresentationAttribute attribute)
        {
            if (Context.PopoverViewController == null)
                return ValueTask.FromResult(false);

            // check for plain popover
            if (Context.PopoverViewController is IMvxIosView iosView && iosView.ViewModel == request.ViewModel)
            {
                return ClosePopoverViewController(Context.PopoverViewController, attribute);
            }

            // check for popover navigation stack
            UIViewController? controllerToClose = null;
            if (Context.PopoverViewController is UINavigationController vc)
            {
                var root = vc.ViewControllers?.FirstOrDefault();
                if (root is IMvxIosView rootIosView && rootIosView.ViewModel == request.ViewModel)
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
            PopoverPresentationAttribute attribute,
            IViewModelRequest request)
        {
            if (Context.PopoverViewController != null)
                throw new AppException($"Trying to show View type: {viewController.GetType().Name} as popover, but there is already a popover present!");

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
                var sourceProvider = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<IPopoverPresentationSourceProvider>();
                sourceProvider?.SetSource(presentationController);
                presentationController.Delegate = new PopoverPresentationControllerDelegate(this);
            }

            Context.PopoverViewController = viewController;

            var parentViewController = GetParentViewController();
            await parentViewController.PresentViewControllerAsync(viewController, attribute.Animated).ConfigureAwait(true);
            return true;
        }

        private async ValueTask<bool> ClosePopoverViewController(UIViewController viewController, PopoverPresentationAttribute attribute)
        {
            if (viewController is UINavigationController { ViewControllers: not null } popoverNavController)
            {
                foreach (var item in popoverNavController.ViewControllers)
                    item.DidMoveToParentViewController(null);
            }

            await viewController.DismissViewControllerAsync(attribute.Animated).ConfigureAwait(true);
            Context.PopoverViewController = null;
            return true;
        }

        // Called if popover was dismissed by tapping outside view.
        public void ClosedPopoverViewController()
        {
            Context.PopoverViewController = null;
        }
    }
}
#endif