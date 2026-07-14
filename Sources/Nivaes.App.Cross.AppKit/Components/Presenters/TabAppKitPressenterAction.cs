using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.AppKitLib;

namespace Nivaes.App.Cross.AppKitOS
{
    public sealed class TabAppKitPressenterAction
        : AppKitPressenterAction<TabPresentationAttribute>
    {
        #region Constructor
        public TabAppKitPressenterAction(
                ICrossViewsContainer viewsContainer,
                IMvxMacViewCreator viewCreator,
                ILogger<TabAppKitPressenterAction> logger)
            : base(viewsContainer, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(Type viewType, TabPresentationAttribute attribute, CrossViewModelRequest request)
        {
            var viewController = (NSViewController)ViewCreator.CreateView(request);

            var window = FindPresentingWindow(attribute.WindowIdentifier, viewController);

            if (window.ContentViewController is not IMvxTabViewController tabViewController)
                throw new CrossException($"Trying to display a tab but there is no TabViewController to host it! View type: {viewController.GetType()}");

            tabViewController.ShowTabView(viewController, attribute.TabTitle);
            return ValueTask.FromResult(true);
        }
    }
}
