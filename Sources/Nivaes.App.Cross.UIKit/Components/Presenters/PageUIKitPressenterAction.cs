using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public abstract class PageUIKitPressenterAction
            : UIKitPressenterAction<MvxPagePresentationAttribute>
    {
        #region Constructor
        public PageUIKitPressenterAction(
                ICrossViewsContainer viewsContainer,
                IMvxIosViewCreator viewCreator,
                ILogger<PageUIKitPressenterAction> logger)
            : base(viewsContainer, viewCreator, logger)
        {
        }
        #endregion

        protected override Task<bool> ShowAction(Type view, MvxPagePresentationAttribute attribute, CrossViewModelRequest request)
        {
            var viewController = (UIViewController?)ViewCreator.CreateView(request);
            if (viewController == null)
            {
                Logger?.LogWarning(
                    "Got null ViewController for request {Request}", request);

                return Task.FromResult(false);
            }
            return ShowPageViewController(viewController, attribute, request);
        }

        protected override Task<bool> CloseAction(ICrossViewModel viewModel, MvxPagePresentationAttribute attribute)
        {
            if (PageViewController != null && PageViewController.RemovePage(viewModel))
                return Task.FromResult(true);

            return Task.FromResult(false);
        }

        private Task<bool> ShowPageViewController(
            UIViewController viewController,
            MvxPagePresentationAttribute attribute,
            CrossViewModelRequest request)
        {
            ArgumentNullException.ThrowIfNull(viewController);
            ArgumentNullException.ThrowIfNull(attribute);

            if (PageViewController == null)
                throw new CrossException("Trying to show a page without a PageViewController, this is not possible!");

            if (attribute.WrapInNavigationController)
                viewController = CreateNavigationController(viewController);

            PageViewController.AddPage(
                viewController,
                attribute);
            return Task.FromResult(true);
        }
    }
}
