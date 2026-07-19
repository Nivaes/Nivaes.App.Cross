using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Windows.UI.Core;

namespace Nivaes.App.Cross.WinUI
{
    public abstract class PressenterAction<TPressenterAttribute>
        : Cross.PressenterAction<TPressenterAttribute>
        where TPressenterAttribute : IPresentationAttribute
    {
        private readonly IPressenterActionContext Context;

        // ToDo: Ha de compartirse con todos los PressenterAction?
        private readonly Lock _windowInformationLock = new();

        // ToDo: Ha de compartirse con todos los PressenterAction?
        private readonly WindowInformation _mainFrame;

        // ToDo: Ha de compartirse con todos los PressenterAction?
        private readonly List<WindowInformation> _windowInformation = new();
        // ToDo: Ha de compartirse con todos los PressenterAction?
        private readonly ICrossWindowsViewModelRequestTranslator _requestTranslator;

        #region Constructor
        public PressenterAction(
                IPressenterActionContext context,
                ICrossWindowsFrame rootFrame,
                ICrossWindowsViewModelRequestTranslator requestTranslator,
                ILogger logger)
            : base(logger)
        {
            Context = context;
            _requestTranslator = requestTranslator;
            var window = (Microsoft.UI.Xaml.Application.Current as CrossWinUIApplication)?.MainWindow;
            //if (window != null)
            //{
            //    window.AppWindow.Closing += (_, __) => CloseAllWindows();
            //}

            _mainFrame = new WindowInformation(window!, rootFrame, null);

            //_logger = CrossLogHost.GetLog<MvxWindowsViewPresenter>();

            //if (Window.Current != null)
            //{
            //    SystemNavigationManager.GetForCurrentView().BackRequested += BackButtonOnBackRequested;
            //}
        }
        #endregion

        protected override BasePresentationAttribute CreatePresentationAttribute(Type? viewModelType, Type? viewType)
        {
            Logger.LogTrace("PresentationAttribute not found for {ViewTypeName}. Assuming new page presentation", viewType?.Name);
            return new PagePresentationAttribute { ViewType = viewType, ViewModelType = viewModelType };
        }

        protected ValueTask<bool> ClosePage(ICrossViewModel viewModel, BasePresentationAttribute attribute)
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

        protected async void CloseWindow(Window newWindow)
        {
            var windowInformation = GetWindowInformation(newWindow);
            if (windowInformation.ViewModel != null)
            {
                await Close(windowInformation.ViewModel);
            }
        }

        /// <summary>
        ///     Gets the correct root frame for the request.
        ///     There is no window or viewmodel for the original root frame.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>The root frame, if no special root frame from a window is found the mainframe is returned.</returns>
        protected WindowInformation GetWindowInformation(ViewModelRequest request)
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
        protected virtual string GetRequestText(ViewModelRequest request)
        {
            //throw new NotImplementedException();
            string requestText;
            requestText = request is ViewModelRequest
                ? _requestTranslator.GetRequestTextWithKeyFor(((ViewModelRequest)request).ViewModel)
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
