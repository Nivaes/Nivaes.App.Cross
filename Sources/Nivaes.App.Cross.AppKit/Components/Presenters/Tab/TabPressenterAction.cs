using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.AppKitLib
{
    public sealed class TabPressenterAction
        : PressenterAction<TabPresentationAttribute>
    {
        #region Constructor
        public TabPressenterAction(
                IPressenterActionContext context,
                IMvxMacViewCreator viewCreator,
                ILogger<TabPressenterAction> logger)
            : base(context, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(Type viewType, TabPresentationAttribute attribute, ViewModelRequest request)
        {
            var viewController = (NSViewController)ViewCreator.CreateView(request);

            var window = FindPresentingWindow(attribute.WindowIdentifier, viewController);

            if (window.ContentViewController is not IMvxTabViewController tabViewController)
                throw new AppException($"Trying to display a tab but there is no TabViewController to host it! View type: {viewController.GetType()}");

            tabViewController.ShowTabView(viewController, attribute.TabTitle);
            return ValueTask.FromResult(true);
        }
    }
}
