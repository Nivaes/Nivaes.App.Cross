using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public sealed class TabPressenterAction
            : PressenterAction<TabPresentationAttribute>
    {
        #region Constructor
        public TabPressenterAction(
                IPressenterActionContext context,
                IMvxIosViewCreator viewCreator,
                ILogger<TabPressenterAction> logger)
            : base(context, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(IViewModelRequest request, TabPresentationAttribute attribute)
        {
            var viewController = (UIViewController?)ViewCreator.CreateView(request);
            if (viewController == null)
            {
                Logger?.LogWarning($"Got null ViewController for request {request}");

                return ValueTask.FromResult(false);
            }
            return ShowTabViewController(viewController, attribute, request);
        }

        protected override ValueTask<bool> CloseAction(IViewModelRequest request, TabPresentationAttribute attribute)
        {
            if (Context.TabBarViewController != null && Context.TabBarViewController.CloseTabViewModel(request.ViewModel))
                return ValueTask.FromResult(true);

            return ValueTask.FromResult(false);
        }

        private ValueTask<bool> ShowTabViewController(
            UIViewController viewController,
            TabPresentationAttribute attribute,
            IViewModelRequest request)
        {
            if (Context.TabBarViewController == null)
                throw new AppException("Trying to show a tab without a TabBarViewController, this is not possible!");

            if (viewController is ITabBarItemViewController tabBarItem)
            {
                attribute.TabName = tabBarItem.TabName;
                attribute.TabIconName = tabBarItem.TabIconName;
                attribute.TabSelectedIconName = tabBarItem.TabSelectedIconName;
            }

            if (attribute.WrapInNavigationController)
                viewController = CreateNavigationController(viewController);

            Context.TabBarViewController.ShowTabView(
                viewController,
                attribute);
            return ValueTask.FromResult(true);
        }
    }
}
