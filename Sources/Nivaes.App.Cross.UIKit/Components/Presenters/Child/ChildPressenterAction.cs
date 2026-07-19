using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public sealed class ChildPressenterAction
            : PressenterAction<ChildPresentationAttribute>
    {
        #region Constructor
        public ChildPressenterAction(
                IPressenterActionContext context,
                IMvxIosViewCreator viewCreator,
                ILogger<ChildPressenterAction> logger)
            : base(context, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(Type viewType, ChildPresentationAttribute attribute, ViewModelRequest request)
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
            if (Context.PopoverViewController is UINavigationController popoverNav &&
                TryCloseViewControllerInsideStack(popoverNav, viewModel, attribute))
            {
                return ValueTask.FromResult(true);
            }
#endif

            // if there are modals presented
            if (Context.ModalViewControllers.Count > 0 && CloseModalChildViewController(viewModel, attribute))
                return ValueTask.FromResult(true);

            // if the current root is a TabBarViewController, delegate close responsibility to it
            if (Context.TabBarViewController?.CloseChildViewModel(viewModel) == true)
                return ValueTask.FromResult(true);

            if (Context.SplitViewController?.CloseChildViewModel(viewModel, attribute) == true)
                return ValueTask.FromResult(true);

            // if the current root is a NavigationController, close it in the stack
            if (Context.MasterNavigationController != null && TryCloseViewControllerInsideStack(Context.MasterNavigationController, viewModel, attribute))
                return ValueTask.FromResult(true);

            return ValueTask.FromResult(false);
        }

        private bool TryCloseViewControllerInsideStack(UINavigationController navController, ICrossViewModel toClose, ChildPresentationAttribute attribute)
        {
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
            foreach (var modalNav in Context.ModalViewControllers.OfType<UINavigationController>())
            {
                if (TryCloseViewControllerInsideStack(modalNav, viewModel, attribute))
                    return true;
            }

            return false;
        }
    }
}
