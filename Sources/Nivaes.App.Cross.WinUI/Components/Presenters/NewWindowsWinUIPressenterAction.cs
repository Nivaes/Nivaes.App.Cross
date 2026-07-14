using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Windows.Graphics;

namespace Nivaes.App.Cross.WinUI
{
    public sealed class NewWindowWinUIPressenterAction
        : PageWinUIPressenterAction<NewWindowPresentationAttribute>
    {
        private const string WindowTitle = "WindowTitle";
        private const int DefaultWindowHeight = 456;
        private const int DefaultWindowWidth = 786;
        private readonly object _windowInformationLock = new();
        private readonly List<WindowInformation> _windowInformation = new();

        #region Constructor
        public NewWindowWinUIPressenterAction(
                ICrossViewsContainer viewsContainer,
                ILogger<NewWindowWinUIPressenterAction> logger)
            : base(viewsContainer, logger)
        {
        }
        #endregion

        protected override async ValueTask<bool> ShowAction(Type viewType, NewWindowPresentationAttribute attribute, CrossViewModelRequest request)
        {
            if (attribute is not NewWindowPresentationAttribute presentationAttribute)
            {
                return false;
            }

            return await ShowNewWindowAsync(request, presentationAttribute);
        }

        protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, NewWindowPresentationAttribute attribute)
        {
            viewModel.ViewDisappearing();
            viewModel.ViewDisappeared();
            viewModel.ViewDestroy();
            viewModel.DisposeIfDisposable();
            return ValueTask.FromResult(true);
        }

        private async Task<bool> ShowNewWindowAsync(CrossViewModelRequest request, NewWindowPresentationAttribute attribute)
        {
            var newWindow = new Window();

            var viewType = base.ViewsContainer?.GetViewType(request.ViewModelType!);
            if (viewType == null)
            {
                Logger.LogError("Could not find View for ViewModelType: {ViewModelType}", request.ViewModelType);
                return false;
            }

            var frame = new CrossWindowsFrame(new Frame());
            await ShowPage(frame, viewType, request);

            newWindow.Content = frame.UnderlyingControl;

            var page = (Page)frame.Content;
            Microsoft.UI.Windowing.AppWindow appWindow = AppWindowUtils.GetAppWindowForCurrentWindow(newWindow);

            if (page.DataContext is ICrossLocalizedTextSourceOwner viewModel)
            {
                appWindow.Title = viewModel.LocalizedTextSource.GetText(WindowTitle);
            }

            // Set size of new window based on the main window.
            if (!AppWindowUtils.TryGetAppWindow(out Microsoft.UI.Windowing.AppWindow? mainWindow) || mainWindow == null)
            {
                Logger.LogWarning("Failed to get App Window");
                return false;
            }

            SizeInt32 size = GetScaledWindowSize(attribute, newWindow, appWindow);
            appWindow.ResizeClient(size);

            // NOTE: This line comes from the community toolkit which is not installed. So we Copied it in.
            await EnqueueAsync(newWindow.DispatcherQueue, newWindow.Activate);

            var model = (ICrossViewModel)page.DataContext;

            lock (_windowInformationLock)
            {
                _windowInformation.Add(new WindowInformation(newWindow, frame, model));
            }

            if (page is IMvxNeedWindow needWindow)
            {
                needWindow.SetWindow(newWindow, appWindow);
            }

            // Closing will be handled before Closed of the Window.
            appWindow.Closing += (_, e) =>
            {
                if (page is IMvxNeedWindow needWindow2 && !needWindow2.CanClose())
                {
                    e.Cancel = true;
                }

            };

            newWindow.Closed += (_, _) =>
            {
                CloseWindow(newWindow);
                page.DisposeIfDisposable();

                lock (_windowInformationLock)
                {
                    _windowInformation.Remove(GetWindowInformation(model));
                }
            };

            return true;
        }

        private static SizeInt32 GetScaledWindowSize(
            NewWindowPresentationAttribute attribute,
            Window newWindow,
            Microsoft.UI.Windowing.AppWindow appWindow)
        {
            double scaleFactor = appWindow.ClientSize.Width / newWindow.Bounds.Width;

            var height = attribute.Height.HasValue
                ? attribute.Height.Value * scaleFactor
                : DefaultWindowHeight * scaleFactor;
            var width = attribute.Width.HasValue
                ? attribute.Width.Value * scaleFactor
                : DefaultWindowWidth * scaleFactor;

            var size = new SizeInt32((int)width, (int)height);
            return size;
        }

        /// <summary>
        ///     Invokes a given function on the target <see cref="DispatcherQueue" /> and returns a
        ///     <see cref="Task" /> that completes when the invocation of the function is completed.
        /// </summary>
        /// <param name="dispatcher">The target <see cref="DispatcherQueue" /> to invoke the code on.</param>
        /// <param name="function">The <see cref="Action" /> to invoke.</param>
        /// <param name="priority">The priority level for the function to invoke.</param>
        /// <returns>A <see cref="Task" /> that completes when the invocation of <paramref name="function" /> is over.</returns>
        /// <remarks>
        ///     If the current thread has access to <paramref name="dispatcher" />, <paramref name="function" /> will be
        ///     invoked directly.
        /// </remarks>
        private static Task EnqueueAsync(DispatcherQueue dispatcher, Action function,
            DispatcherQueuePriority priority = DispatcherQueuePriority.Normal)
        {
            // Run the function directly when we have thread access.
            // Also reuse Task.CompletedTask in case of success,
            // to skip an unnecessary heap allocation for every invocation.
            if (dispatcher.HasThreadAccess)
            {
                try
                {
                    function();

                    return Task.CompletedTask;
                }
                catch (Exception e)
                {
                    return Task.FromException(e);
                }
            }

            static Task TryEnqueue(DispatcherQueue dispatcher, Action function, DispatcherQueuePriority priority)
            {
                var taskCompletionSource =
                    new TaskCompletionSource<object?>(TaskCreationOptions.RunContinuationsAsynchronously);

                if (!dispatcher.TryEnqueue(
                        priority,
                        () =>
                        {
                            try
                            {
                                function();

                                taskCompletionSource.SetResult(null);
                            }
                            catch (Exception e)
                            {
                                taskCompletionSource.SetException(e);
                            }
                        }))
                {
                    taskCompletionSource.SetException(new Exception("Failed to enqueue the operation"));
                }

                return taskCompletionSource.Task;
            }

            return TryEnqueue(dispatcher, function, priority);
        }

    }
}
