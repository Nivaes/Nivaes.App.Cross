using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public sealed class PagePressenterAction
            : PressenterAction<PagePresentationAttribute>
    {
        #region Constructor
        public PagePressenterAction(
                IPressenterActionContext context,
                IMvxIosViewCreator viewCreator,
                ILogger<PagePressenterAction> logger)
            : base(context, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(PagePresentationAttribute attribute, IViewModelRequest request)
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
            if (Context.PageViewController != null && Context.PageViewController.RemovePage(viewModel))
                return ValueTask.FromResult(true);

            return ValueTask.FromResult(false);
        }

        private ValueTask<bool> ShowPageViewController(
            UIViewController viewController,
            PagePresentationAttribute attribute,
            IViewModelRequest request)
        {
            if (Context.PageViewController == null)
                throw new AppException("Trying to show a page without a PageViewController, this is not possible!");

            if (attribute.WrapInNavigationController)
                viewController = CreateNavigationController(viewController);

            Context.PageViewController.AddPage(
                viewController,
                attribute);
            return ValueTask.FromResult(true);
        }
    }
}
