using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.WinUI
{
    public abstract class PageWinUIPressenterAction<TPressenterAttribute>
        : PressenterAction<TPressenterAttribute>
        where TPressenterAttribute : IPresentationAttribute
    {
        #region Constructor
        public PageWinUIPressenterAction(
                IPressenterActionContext context,
                ICrossWindowsFrame rootFrame,
                ICrossWindowsViewModelRequestTranslator requestTranslator,
                ILogger logger)
            : base(context, rootFrame, requestTranslator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(Type viewType, TPressenterAttribute attribute, ViewModelRequest request)
        {
            return ShowPage(GetWindowInformation(request).RootFrame, viewType, request);
        }

        /// <summary>
        ///     Shows a page for the given request.
        /// </summary>
        /// <param name="rootFrame">The root frame to show the page in.</param>
        /// <param name="viewType">The type of the content.</param>
        /// <param name="request">The request to show the page.</param>
        /// <returns>True if successful, false otherwise.</returns>
        protected ValueTask<bool> ShowPage(ICrossWindowsFrame rootFrame, Type viewType, ViewModelRequest request)
        {
            try
            {
                var requestText = GetRequestText(request);

                //Frame won't allow serialization of it's nav-state if it gets a non-simple type as a nav param
                rootFrame.Navigate(viewType, requestText);

                HandleBackButtonVisibility();
                return ValueTask.FromResult(true);
            }
            catch (Exception exception)
            {
                var message = request.ViewModelType?.Name ?? "(No view model type specified)";
                throw new AppException(exception, $"Error seen during navigation request to {message}.");
            }
        }
    }

    public sealed class PageWinUIPressenterAction
        : PageWinUIPressenterAction<PagePresentationAttribute>
    {
        #region Constructor
        public PageWinUIPressenterAction(
                IPressenterActionContext context,
                ICrossWindowsFrame rootFrame,
                ICrossWindowsViewModelRequestTranslator requestTranslator,
                ILogger<PageWinUIPressenterAction> logger)
            : base(context, rootFrame, requestTranslator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, PagePresentationAttribute attribute)
        {
            return ClosePage(viewModel, attribute); 
        }
    }
}
