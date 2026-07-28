#if IOS || MACCATALYST
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public sealed class FullPressenterAction
            : PressenterAction<FullPresentationAttribute>
    {
        #region Constructor
        public FullPressenterAction(
                IPressenterActionContext context,
                IMvxIosViewCreator viewCreator,
                ILogger<FullPressenterAction> logger)
            : base(context, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(IViewModelRequest request, FullPresentationAttribute attribute)
        {
            var viewController = (UIViewController)ViewCreator.CreateView(request);
            return ShowFullViewController(viewController, attribute, request);
        }

        protected override ValueTask<bool> CloseAction(IViewModelRequest request, FullPresentationAttribute attribute)
        {
            Logger.LogWarning($"Ignored attempt to close the window root (ViewModel type: {request.ViewModelType.Name}");

            return ValueTask.FromResult(false);
        }

        private async ValueTask<bool> ShowFullViewController(
           UIViewController viewController,
           FullPresentationAttribute attribute,
           IViewModelRequest request)
        {
            // check if viewController is a TabBarController
            if (viewController is ITabBarViewController tabBarController)
            {
                Context.TabBarViewController = tabBarController;

                // set root
                SetupFullWindowsNavigation(viewController, attribute);

                await CloseModalViewControllers();
                await CloseSplitViewController();

                return true;
            }

            SetupFullWindowsNavigation(viewController, attribute);

            await base.CloseModalViewControllers();
            await base.CloseTabBarViewController();
            await base.CloseSplitViewController();

            return true;
        }

        private void SetupFullWindowsNavigation(UIViewController viewController, FullPresentationAttribute attribute)
        {
            //base.MasterNavigationController = MainNavitagionController = base.CreateNavigationController(viewController);

            CreateSlideMenuController(Context.MasterNavigationController);

            viewController.AddLeftBarButtonWithImage(UIImage.FromBundle("ic_menu"));
        }
    }
}
#endif