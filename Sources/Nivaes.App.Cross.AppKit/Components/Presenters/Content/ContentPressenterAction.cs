using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.AppKitLib
{
    public sealed class ContentPressenterAction
        : PressenterAction<ContentPresentationAttribute>
    {
        #region Constructor
        public ContentPressenterAction(
                IPressenterActionContext context,
                IMvxMacViewCreator viewCreator,
                ILogger<ContentPressenterAction> logger)
            : base(context, viewCreator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(Type viewType, ContentPresentationAttribute attribute, ViewModelRequest request)
        { 
            var viewController = (NSViewController)ViewCreator.CreateView(request);

            var window = FindPresentingWindow(attribute.WindowIdentifier, viewController);

            if (!string.IsNullOrEmpty(viewController.Title))
                window.Title = viewController.Title;

            window.ContentView = viewController.View;
            window.ContentViewController = viewController;
            return ValueTask.FromResult(true);
        }
    }
}
