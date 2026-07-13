using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public abstract class TabUIKitPressenterAction
            : UIKitPressenterAction<MvxTabPresentationAttribute>
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

        protected override Task<bool> ShowAction(Type view, MvxTabPresentationAttribute attribute, CrossViewModelRequest request)
        {
            var viewController = (UIViewController?)ViewCreator.CreateView(request);
            if (viewController == null)
            {
                Logger?.LogWarning(
                    "Got null ViewController for request {Request}", request);

                return Task.FromResult(false);
            }
            return ShowTabViewController(viewController, attribute, request);
        }

        protected override Task<bool> CloseAction(ICrossViewModel viewModel, MvxTabPresentationAttribute attribute)
        {
            if (TabBarViewController != null && TabBarViewController.CloseTabViewModel(viewModel))
                return Task.FromResult(true);

            return Task.FromResult(false);
        }

        private Task<bool> ShowTabViewController(
            UIViewController viewController,
            MvxTabPresentationAttribute attribute,
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
            return Task.FromResult(true);
        }
    }
}
