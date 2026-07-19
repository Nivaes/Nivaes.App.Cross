using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace Nivaes.App.Cross.WinUI
{
    public sealed class DialogWinUIPressenterAction
        : PressenterAction<DialogViewPresentationAttribute>
    {
        private readonly IServiceProvider _serviceProvider;

        #region Constructor
        public DialogWinUIPressenterAction(
                IPressenterActionContext context,
                IServiceProvider serviceProvider,
                ICrossWindowsFrame rootFrame,
                ICrossWindowsViewModelRequestTranslator requestTranslator,
                ILogger<DialogWinUIPressenterAction> logger)
            : base(context, rootFrame, requestTranslator, logger)
        {
            _serviceProvider = serviceProvider;
        }
        #endregion

        protected override async ValueTask<bool> ShowAction(Type viewType, DialogViewPresentationAttribute attribute, ViewModelRequest request)
        {
            try
            {
                var contentDialog = CreateControl(viewType, request, attribute) as ContentDialog;

                if (contentDialog != null)
                {
                    var windowInfo = GetWindowInformation(request);
                    if (windowInfo.RootFrame.UnderlyingControl is Frame frame)
                    {
                        contentDialog.XamlRoot = frame.XamlRoot;
                    }

                    await contentDialog.ShowAsync(attribute.Placement);
                    if (contentDialog is ICrossView mvxControl && mvxControl.ViewModel != null)
                    {
                        windowInfo.RegisterSubViewModel(mvxControl.ViewModel);
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

        protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, DialogViewPresentationAttribute attribute)
        {
            var windowInformation = GetWindowInformation(viewModel);
            if (windowInformation.RootFrame.UnderlyingControl is not Frame frame)
            {
                return ValueTask.FromResult(false);
            }

            var popups = VisualTreeHelper.GetOpenPopupsForXamlRoot(frame.XamlRoot).FirstOrDefault(p =>
            {
                if (attribute.ViewType != null && attribute.ViewType.IsInstanceOfType(p.Child)
                                               && p.Child is ICrossWindowsContentDialog dialog)
                {
                    return dialog.ViewModel == viewModel;
                }

                return false;
            });

            (popups?.Child as ContentDialog)?.Hide();
            windowInformation.UnregisterSubViewModel(viewModel);
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
        private Control? CreateControl(Type viewType, ViewModelRequest request,
            BasePresentationAttribute attribute)
        {
            try
            {
                var control = ActivatorUtilities.CreateInstance(_serviceProvider, viewType) as Control;
                if (control is ICrossView mvxControl)
                {
                    //if (request is CrossViewModelInstanceRequest instanceRequest)
                    //{
                    //    mvxControl.ViewModel = instanceRequest.ViewModelInstance;
                    //}
                    //else
                    //{
                    //    mvxControl.ViewModel = _viewModelLoader?.LoadViewModel(request, null);
                    //}
                    mvxControl.ViewModel = request.ViewModel;
                }

                return control;
            }
            catch (Exception ex)
            {
                throw new AppException(ex,
                    $"Cannot create Control '{viewType.FullName}'. Are you use the wrong base class?");
            }
        }
    }
}
