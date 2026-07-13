using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.AppKitLib;

namespace Nivaes.App.Cross.AppKitOS
{
    public sealed class ModalMacPressenterAction
        : MacPressenterAction<CrossModalPresentationAttribute>
    {
        #region Constructor
        public ModalMacPressenterAction(
                ICrossViewsContainer viewsContainer,
                IMvxMacViewCreator viewCreator,
                ILogger<ModalMacPressenterAction> logger)
            : base(viewsContainer, viewCreator, logger)
        {
        }
        #endregion

        protected override Task<bool> ShowAction(Type view, CrossModalPresentationAttribute attribute, CrossViewModelRequest request)
        {
            var viewController = (NSViewController)ViewCreator.CreateView(request);

            var window = FindPresentingWindow(attribute.WindowIdentifier, viewController);

            window.ContentViewController.PresentViewControllerAsModalWindow(viewController);
            return Task.FromResult(true);
        }
    }
}
