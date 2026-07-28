using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.WinUI
{
    public abstract class PagePressenterAction<TPressenterAttribute>
        : PressenterAction<TPressenterAttribute>
        where TPressenterAttribute : IPresentationAttribute
    {
        #region Constructor
        public PagePressenterAction(
                IPressenterActionContext context,
                ICrossWindowsFrame rootFrame,
                ILogger logger)
            : base(context, rootFrame, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(IViewModelRequest request, TPressenterAttribute attribute)
        {
            return ShowPage(GetWindowInformation(request).RootFrame, request.ViewType, request);
        }

        /// <summary>
        ///     Shows a page for the given request.
        /// </summary>
        /// <param name="rootFrame">The root frame to show the page in.</param>
        /// <param name="viewType">The type of the content.</param>
        /// <param name="request">The request to show the page.</param>
        /// <returns>True if successful, false otherwise.</returns>
        protected ValueTask<bool> ShowPage(ICrossWindowsFrame rootFrame, Type viewType, IViewModelRequest request)
        {
            try
            {
                var requestBuffer = ViewModelRequestSerializer.Serializer(request);

                rootFrame.Navigate(viewType, requestBuffer);

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
        : PagePressenterAction<PagePresentationAttribute>
    {
        #region Constructor
        public PageWinUIPressenterAction(
                IPressenterActionContext context,
                ICrossWindowsFrame rootFrame,
                ILogger<PageWinUIPressenterAction> logger)
            : base(context, rootFrame, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> CloseAction(IViewModelRequest request, PagePresentationAttribute attribute)
        {
            return ClosePage(request.ViewModel, attribute); 
        }
    }
}
