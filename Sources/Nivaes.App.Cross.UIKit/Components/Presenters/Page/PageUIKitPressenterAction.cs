using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public sealed class PageUIKitPressenterAction
            : PressenterAction<PagePresentationAttribute>
    {
        #region Constructor
        public PageUIKitPressenterAction(
                PressenterActionContext context,
                ICrossViewsContainer viewsContainer,
                IMvxIosViewCreator viewCreator,
                ILogger<PageUIKitPressenterAction> logger)
            : base(context, viewsContainer, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(Type viewType, PagePresentationAttribute attribute, CrossViewModelRequest request)
        {
            var viewController = (UIViewController?)ViewCreator.CreateView(request);
            if (viewController == null)
            {
                Logger?.LogWarning(
                    "Got null ViewController for request {Request}", request);

                return ValueTask.FromResult(false);
            }
            return ShowPageViewController(viewController, attribute, request);
        }

        protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, PagePresentationAttribute attribute)
        {
            if (PageViewController != null && PageViewController.RemovePage(viewModel))
                return ValueTask.FromResult(true);

            return ValueTask.FromResult(false);
        }

        private ValueTask<bool> ShowPageViewController(
            UIViewController viewController,
            PagePresentationAttribute attribute,
            CrossViewModelRequest request)
        {
            ArgumentNullException.ThrowIfNull(viewController);
            ArgumentNullException.ThrowIfNull(attribute);

            if (PageViewController == null)
                throw new AppException("Trying to show a page without a PageViewController, this is not possible!");

            if (attribute.WrapInNavigationController)
                viewController = CreateNavigationController(viewController);

            PageViewController.AddPage(
                viewController,
                attribute);
            return ValueTask.FromResult(true);
        }
    }
}
