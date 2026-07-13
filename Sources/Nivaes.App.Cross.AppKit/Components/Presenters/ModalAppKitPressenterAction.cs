using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.AppKitLib;

namespace Nivaes.App.Cross.AppKitOS
{
    public sealed class ModalAppKitPressenterAction
        : AppKitPressenterAction<CrossModalPresentationAttribute>
    {
        #region Constructor
        public ModalAppKitPressenterAction(
                ICrossViewsContainer viewsContainer,
                IMvxMacViewCreator viewCreator,
                ILogger<ModalAppKitPressenterAction> logger)
            : base(viewsContainer, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(Type viewType, CrossModalPresentationAttribute attribute, CrossViewModelRequest request)
        {
            var viewController = (NSViewController)ViewCreator.CreateView(request);

            var window = FindPresentingWindow(attribute.WindowIdentifier, viewController);

            window.ContentViewController.PresentViewControllerAsModalWindow(viewController);
            return ValueTask.FromResult(true);
        }
    }
}
