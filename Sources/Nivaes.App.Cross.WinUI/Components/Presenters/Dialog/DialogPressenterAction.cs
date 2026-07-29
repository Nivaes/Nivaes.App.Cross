using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace Nivaes.App.Cross.WinUI
{
    public sealed class DialogPressenterAction
        : PressenterAction<DialogViewPresentationAttribute>
    {
        private readonly IServiceProvider _serviceProvider;

        #region Constructor
        public DialogPressenterAction(
                IPressenterActionContext context,
                IServiceProvider serviceProvider,
                ICrossWindowsFrame rootFrame,
                ILogger<DialogPressenterAction> logger)
            : base(context, rootFrame, logger)
        {
            _serviceProvider = serviceProvider;
        }
        #endregion

        protected override async ValueTask<bool> ShowAction(IViewModelRequest request, DialogViewPresentationAttribute attribute)
        {
            try
            {
                var content = CreateControl(request, attribute);
                var contentDialog = content as ContentDialog;
                if (contentDialog == null)
                {
                    contentDialog = new ContentDialog();
                    contentDialog.Content = content;
                }

                if (contentDialog != null)
                {
                    var windowInfo = GetWindowInformation(request);
                    if (windowInfo.RootFrame.UnderlyingControl is Frame frame)
                    {
                        contentDialog.XamlRoot = frame.XamlRoot;
                    }

                    await contentDialog.ShowAsync(attribute.Placement);
                    if (contentDialog is ICrossView controlView && controlView.ViewModel != null)
                    {
                        windowInfo.RegisterSubViewModel(controlView.ViewModel);
                    }

                    return true;
                }

                return false;
            }
            catch (Exception exception)
            {
                Logger?.LogError(exception, "Error seen during navigation request to {ViewModelTypeName}",
                    request.ViewModelType?.Name);
                return false;
            }
        }

        protected override ValueTask<bool> CloseAction(IViewModelRequest request, DialogViewPresentationAttribute attribute)
        {
            var windowInformation = GetWindowInformation(request.ViewModel);
            if (windowInformation.RootFrame.UnderlyingControl is not Frame frame)
            {
                return ValueTask.FromResult(false);
            }

            var popups = VisualTreeHelper.GetOpenPopupsForXamlRoot(frame.XamlRoot).FirstOrDefault(p =>
            {
                if (request.ViewType != null && request.ViewType.IsInstanceOfType(p.Child)
                                               && p.Child is ICrossWindowsContentDialog dialog)
                {
                    return dialog.ViewModel == request.ViewModel;
                }

                return false;
            });

            (popups?.Child as ContentDialog)?.Hide();
            windowInformation.UnregisterSubViewModel(request.ViewModel);
            return ValueTask.FromResult(true);
        }

        /// <summary>
        ///     Creates a control for the given view type.
        /// </summary>
        /// <param name="viewType">The view type.</param>
        /// <param name="request">The request.</param>
        /// <param name="attribute">Any attributes.</param>
        /// <returns></returns>
        /// <exception cref="AppException"></exception>
        private Control? CreateControl(IViewModelRequest request, BasePresentationAttribute attribute)
        {
            try
            {
                var control = ActivatorUtilities.CreateInstance(_serviceProvider, request.ViewType) as Control;
                if (control is ICrossView controlView)
                {
                    controlView.ViewModel = request.ViewModel;
                }

                return control;
            }
            catch (Exception ex)
            {
                throw new AppException(ex, $"Cannot create Control '{request.ViewType.FullName}'. Are you use the wrong base class?");
            }
        }
    }
}
