using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.AppKitLib
{
    public sealed class SheetMacPressenterAction
        : PressenterAction<SheetPresentationAttribute>
    {
        #region Constructor
        public SheetMacPressenterAction(
                IPressenterActionContext context,
                IMvxMacViewCreator viewCreator,
                ILogger<SheetMacPressenterAction> logger)
            : base(context, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(IViewModelRequest request, SheetPresentationAttribute attribute)
        {
            var viewController = (NSViewController)ViewCreator.CreateView(request);

            var window = FindPresentingWindow(attribute.WindowIdentifier, viewController);

            window.ContentViewController.PresentViewControllerAsSheet(viewController);
            return ValueTask.FromResult(true);
        }
    }
}
