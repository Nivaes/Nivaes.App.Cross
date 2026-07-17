#if IOS || MACCATALYST
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public sealed class FullPressenterAction
            : PressenterAction<FullPresentationAttribute>
    {
        #region Constructor
        public FullPressenterAction(
                PressenterActionContext context,
                ICrossViewsContainer viewsContainer,
                IMvxIosViewCreator viewCreator,
                ILogger<FullPressenterAction> logger)
            : base(context, viewsContainer, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(Type viewType, FullPresentationAttribute attribute, CrossViewModelRequest request)
        {
            var viewController = (UIViewController)ViewCreator.CreateView(request);
            return ShowFullViewController(viewController, attribute, request);
        }

        protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, FullPresentationAttribute attribute)
        {
            base.Logger.LogWarning($"Ignored attempt to close the window root (ViewModel type: {viewModel.GetType().Name}");

            return ValueTask.FromResult(false);
        }

        private async ValueTask<bool> ShowFullViewController(
           UIViewController viewController,
           FullPresentationAttribute attribute,
           CrossViewModelRequest request)
        {
            // check if viewController is a TabBarController
            if (viewController is IMvxTabBarViewController tabBarController)
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

            CreateSlideMenuController(base.MasterNavigationController);

            viewController.AddLeftBarButtonWithImage(UIImage.FromBundle("ic_menu"));
        }
    }
}
#endif