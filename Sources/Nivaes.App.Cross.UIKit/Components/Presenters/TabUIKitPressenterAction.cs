using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public abstract class TabUIKitPressenterAction
            : UIKitPressenterAction<TabPresentationAttribute>
    {
        #region Constructor
        public TabUIKitPressenterAction(
                ICrossViewsContainer viewsContainer,
                IMvxIosViewCreator viewCreator,
                ILogger<TabUIKitPressenterAction> logger)
            : base(viewsContainer, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(Type viewType, TabPresentationAttribute attribute, CrossViewModelRequest request)
        {
            var viewController = (UIViewController?)ViewCreator.CreateView(request);
            if (viewController == null)
            {
                Logger?.LogWarning(
                    "Got null ViewController for request {Request}", request);

                return ValueTask.FromResult(false);
            }
            return ShowTabViewController(viewController, attribute, request);
        }

        protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, TabPresentationAttribute attribute)
        {
            if (TabBarViewController != null && TabBarViewController.CloseTabViewModel(viewModel))
                return ValueTask.FromResult(true);

            return ValueTask.FromResult(false);
        }

        private ValueTask<bool> ShowTabViewController(
            UIViewController viewController,
            TabPresentationAttribute attribute,
            CrossViewModelRequest request)
        {
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
            return ValueTask.FromResult(true);
        }
    }
}
