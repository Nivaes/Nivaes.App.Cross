using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.AppKitLib
{
    public sealed class ModalPressenterAction
        : PressenterAction<ModalPresentationAttribute>
    {
        #region Constructor
        public ModalPressenterAction(
                IPressenterActionContext context,
                IMvxMacViewCreator viewCreator,
                ILogger<ModalPressenterAction> logger)
            : base(context, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(IViewModelRequest request, ModalPresentationAttribute attribute)
        {
            var viewController = (NSViewController)ViewCreator.CreateView(request);

            var window = FindPresentingWindow(attribute.WindowIdentifier, viewController);

            window.ContentViewController.PresentViewControllerAsModalWindow(viewController);
            return ValueTask.FromResult(true);
        }
    }
}
