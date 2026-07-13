using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Windows.UI.Core;

namespace Nivaes.App.Cross.WinUI
{
    public abstract class WinUIPressenterAction<TPressenterAttribute> 
        : PressenterAction<TPressenterAttribute>
        where TPressenterAttribute : ICrossPresentationAttribute
    {
        // ToDo: Ha de compartirse con todos los PressenterAction?
        private readonly object _windowInformationLock = new();

        // ToDo: Ha de compartirse con todos los PressenterAction?
        private readonly WindowInformation _mainFrame;

        // ToDo: Ha de compartirse con todos los PressenterAction?
        private readonly List<WindowInformation> _windowInformation = new();
        // ToDo: Ha de compartirse con todos los PressenterAction?
        private readonly ICrossWindowsViewModelRequestTranslator _requestTranslator;

        #region Constructor
        public WinUIPressenterAction(
                ICrossViewsContainer viewsContainer,
                ILogger logger)
            : base(viewsContainer, logger)
        {
            //_mainFrame = new WindowInformation(window!, rootFrame, null);
        }
        #endregion

        protected override CrossBasePresentationAttribute CreatePresentationAttribute(Type? viewModelType, Type? viewType)
        {
            Logger.LogTrace("PresentationAttribute not found for {ViewTypeName}. Assuming new page presentation",  viewType?.Name);
            return new MvxPagePresentationAttribute { ViewType = viewType, ViewModelType = viewModelType };
        }

        protected ValueTask<bool> ClosePage(ICrossViewModel viewModel, CrossBasePresentationAttribute attribute)
        {
            var windowInformation = GetWindowInformation(viewModel);
            var currentView = windowInformation.RootFrame.Content as ICrossView;
            if (currentView == null)
            {
                Logger?.LogWarning("Ignoring close for viewmodel - root frame has no current page");
                return ValueTask.FromResult(false);
            }

            if (currentView.ViewModel != viewModel)
            {
                Logger?.LogWarning(
                    "Ignoring close for viewmodel - root frame's current page is not the view for the requested viewmodel");
                return ValueTask.FromResult(false);
            }

            if (!windowInformation.RootFrame.CanGoBack)
            {
                Logger?.LogWarning("Ignoring close for viewmodel - root frame refuses to go back");
                return ValueTask.FromResult(false);
            }

            windowInformation.RootFrame.GoBack();

            HandleBackButtonVisibility();
            windowInformation.UnregisterSubViewModel(viewModel);

            return ValueTask.FromResult(true);
        }

        protected void CloseWindow(Window newWindow)
        {
            var windowInformation = GetWindowInformation(newWindow);
            if (windowInformation.ViewModel != null)
            {
                Close(windowInformation.ViewModel);
            }
        }

        /// <summary>
        ///     Gets the correct root frame for the request.
        ///     There is no window or viewmodel for the original root frame.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The root frame, if no special root frame from a window is found the mainframe is returned.</returns>
        protected WindowInformation GetWindowInformation(CrossViewModelRequest request)
        {
            lock (_windowInformationLock)
            {
                var frame = _mainFrame;
                if (request is CrossViewModelInstanceRequestWithSource targetRequest)
                {
                    frame = _windowInformation.Find(wi => wi.IsFor(targetRequest.Source)) ??
                            _mainFrame;
                }

                return frame;
            }
        }

        /// <summary>
        ///     Gets the correct root frame for the request.
        ///     There is no window or viewmodel for the original root frame.
        /// </summary>
        /// <param name="viewModel">The viewmodel to get the root frame for.</param>
        /// <returns>The root frame, if no special root frame from a window is found the mainframe is returned.</returns>
        protected WindowInformation GetWindowInformation(ICrossViewModel viewModel)
        {
            lock (_windowInformationLock)
            {
                return _windowInformation.Find(wi => wi.IsFor(viewModel)) ?? _mainFrame;
            }
        }

        /// <summary>
        ///     Gets the correct root frame for the request.
        ///     There is no window or viewmodel for the original root frame.
        /// </summary>
        /// <param name="window">The window to get the root frame for.</param>
        /// <returns>The root frame, if no special root frame from a window is found the mainframe is returned.</returns>
        protected WindowInformation GetWindowInformation(Window window)
        {
            lock (_windowInformationLock)
            {
                return _windowInformation.Find(wi => wi.IsFor(window)) ?? _mainFrame;
            }
        }

        /// <summary>
        ///     Converts a request to a string format.
        ///     A Frame won't allow serialization of it's nav-state if it gets a non-simple type as a nav param
        /// </summary>
        /// <param name="request">The request to convert.</param>
        /// <returns>A text representation of the request.</returns>
        protected virtual string GetRequestText(CrossViewModelRequest request)
        {
            string requestText;
            requestText = request is CrossViewModelInstanceRequest
                ? _requestTranslator.GetRequestTextWithKeyFor(((CrossViewModelInstanceRequest)request).ViewModelInstance!)
                : _requestTranslator.GetRequestTextFor(request);

            return requestText;
        }

        /// <summary>
        ///     Updates the visibility state of the back button.
        /// </summary>
        protected void HandleBackButtonVisibility()
        {
            if (Window.Current == null)
            {
                return;
            }

            var rootFrame = GetWindowInformation(Window.Current).RootFrame;

            SystemNavigationManager.GetForCurrentView().AppViewBackButtonVisibility =
                rootFrame.CanGoBack ? AppViewBackButtonVisibility.Visible : AppViewBackButtonVisibility.Collapsed;
        }
    }
}
