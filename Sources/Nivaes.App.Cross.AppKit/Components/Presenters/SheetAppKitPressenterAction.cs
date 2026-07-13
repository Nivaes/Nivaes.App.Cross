using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.AppKitLib;

namespace Nivaes.App.Cross.AppKitOS
{
    public sealed class wheetMacPressenterAction
        : AppKitPressenterAction<MvxSheetPresentationAttribute>
    {
        #region Constructor
        public wheetMacPressenterAction(
                ICrossViewsContainer viewsContainer,
                IMvxMacViewCreator viewCreator,
                ILogger<wheetMacPressenterAction> logger)
            : base(viewsContainer, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(Type viewType, MvxSheetPresentationAttribute attribute, CrossViewModelRequest request)
        {
            var viewController = (NSViewController)ViewCreator.CreateView(request);

            var window = FindPresentingWindow(attribute.WindowIdentifier, viewController);

            window.ContentViewController.PresentViewControllerAsSheet(viewController);
            return ValueTask.FromResult(true);
        }
    }
}
