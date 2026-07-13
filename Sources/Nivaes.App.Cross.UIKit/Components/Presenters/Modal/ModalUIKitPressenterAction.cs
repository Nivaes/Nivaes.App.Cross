using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public abstract class ModalUIKitPressenterAction
            : UIKitPressenterAction<MvxModalPresentationAttribute>
    {
        #region Constructor
        public ModalUIKitPressenterAction(
                ICrossViewsContainer viewsContainer,
                IMvxIosViewCreator viewCreator,
                ILogger<ModalUIKitPressenterAction> logger)
            : base(viewsContainer, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(Type viewType, MvxModalPresentationAttribute attribute, CrossViewModelRequest request)
        {
            var viewController = (UIViewController?)ViewCreator.CreateView(request);
            if (viewController == null)
            {
                Logger?.LogWarning(
                    "Got null ViewController for request {Request}", request);

                return ValueTask.FromResult(false);
            }
            return ShowModalViewController(viewController, attribute, request);
        }

        protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, MvxModalPresentationAttribute attribute)
        {
            if (ModalViewControllers.Count == 0)
                return ValueTask.FromResult(false);

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

            return ValueTask.FromResult(false);
        }

        protected virtual ValueTask<bool> ShowModalViewController(
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

            return ValueTask.FromResult(true);
        }

        protected virtual IUIAdaptivePresentationControllerDelegate CreateModalPresentationControllerDelegate(
           UIViewController viewController, MvxModalPresentationAttribute attribute)
        {
            return new ModalPresentationControllerDelegate(this, viewController, attribute);
        }
    }
}
