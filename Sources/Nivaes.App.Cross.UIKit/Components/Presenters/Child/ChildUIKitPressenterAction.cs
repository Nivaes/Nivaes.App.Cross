using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public abstract class ChildUIKitPressenterAction
            : UIKitPressenterAction<ChildPresentationAttribute>
    {
        #region Constructor
        public ChildUIKitPressenterAction(
                ICrossViewsContainer viewsContainer,
                IMvxIosViewCreator viewCreator,
                ILogger<ChildUIKitPressenterAction> logger)
            : base(viewsContainer, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(Type viewType, ChildPresentationAttribute attribute, CrossViewModelRequest request)
        {
            var viewController = (UIViewController?)ViewCreator.CreateView(request);
            if (viewController == null)
            {
                Logger.LogWarning(
                    "Got null ViewController for request {Request}", request);

                return ValueTask.FromResult(false);
            }
            return ShowChildViewController(viewController, attribute, request);
        }

        protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, ChildPresentationAttribute attribute)
        {
#if IOS || MACCATALYST
            // if a popover is presented
            if (PopoverViewController is UINavigationController popoverNav &&
                TryCloseViewControllerInsideStack(popoverNav, viewModel, attribute))
            {
                return ValueTask.FromResult(true);
            }
#endif

            // if there are modals presented
            if (ModalViewControllers.Count > 0 && CloseModalChildViewController(viewModel, attribute))
                return ValueTask.FromResult(true);

            // if the current root is a TabBarViewController, delegate close responsibility to it
            if (TabBarViewController?.CloseChildViewModel(viewModel) == true)
                return ValueTask.FromResult(true);

            if (SplitViewController?.CloseChildViewModel(viewModel, attribute) == true)
                return ValueTask.FromResult(true);

            // if the current root is a NavigationController, close it in the stack
            if (MasterNavigationController != null && TryCloseViewControllerInsideStack(MasterNavigationController, viewModel, attribute))
                return ValueTask.FromResult(true);

            return ValueTask.FromResult(false);
        }

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

        private bool CloseModalChildViewController(ICrossViewModel viewModel, ChildPresentationAttribute attribute)
        {
            foreach (var modalNav in ModalViewControllers.OfType<UINavigationController>())
            {
                if (TryCloseViewControllerInsideStack(modalNav, viewModel, attribute))
                    return true;
            }

            return false;
        }
    }
}
