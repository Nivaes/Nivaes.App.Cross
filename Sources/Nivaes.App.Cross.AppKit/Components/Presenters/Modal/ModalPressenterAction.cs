using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.AppKitLib
{
    public sealed class ModalPressenterAction
        : AppKitPressenterAction<ModalPresentationAttribute>
    {
        #region Constructor
        public ModalPressenterAction(
                ICrossViewsContainer viewsContainer,
                IMvxMacViewCreator viewCreator,
                ILogger<ModalPressenterAction> logger)
            : base(viewsContainer, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(Type viewType, ModalPresentationAttribute attribute, CrossViewModelRequest request)
        {
            var viewController = (NSViewController)ViewCreator.CreateView(request);

            var window = FindPresentingWindow(attribute.WindowIdentifier, viewController);

            window.ContentViewController.PresentViewControllerAsModalWindow(viewController);
            return ValueTask.FromResult(true);
        }
    }
}
