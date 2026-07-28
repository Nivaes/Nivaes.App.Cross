using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

namespace Nivaes.App.Cross.WinUI
{
    public class ShellPagePressenterAction
        : PressenterAction<ShellPagePresentationAttribute>
    {
        #region Constructor
        public ShellPagePressenterAction(
                IPressenterActionContext context,
                ICrossWindowsFrame rootFrame,
                ILogger<ShellPagePressenterAction> logger)
            : base(context, rootFrame, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(IViewModelRequest request, ShellPagePresentationAttribute attribute)
        {
            var windowInformation = GetWindowInformation(request);

            if (windowInformation.RootFrame.Content is ShellView shellView)
            {
                var containerView = shellView.RootFrame;

                windowInformation.RegisterSubViewModel(request.ViewModel);

                if (containerView != null)
                {
                    var requestBuffer = ViewModelRequestSerializer.Serializer(request);

                    containerView.Navigate(request.ViewType, requestBuffer, new SuppressNavigationTransitionInfo());

                    containerView.HorizontalAlignment = HorizontalAlignment.Stretch;
                    return ValueTask.FromResult(true);
                }
                else
                {
                    //throw new AppException($"Not containerView found for {viewType.FullName}");
                    Logger.LogError($"Not containerView found for {request.ViewType.FullName}");
                    return ValueTask.FromResult(false);
                }
            }
            else
            {
                Logger.LogError($"Not {nameof(ShellView)} found to pressenter {request.ViewType.FullName}");
                return ValueTask.FromResult(false);
            }
        }

        protected override ValueTask<bool> CloseAction(IViewModelRequest request, ShellPagePresentationAttribute attribute)
        {
            var windowInformation = GetWindowInformation(request.ViewModel);

            var viewType = Singleton<ViewModelViewsKeyContainerManager>.Instance.GetValue(request.ViewModel.GetType());

            var shellView = windowInformation.RootFrame.Content as ShellView;

            var containerView = shellView?.RootFrame;

            if (containerView == null)
            {
                // This can happen if a parent view is already removed.
                return ValueTask.FromResult(false);
            }

            if (containerView.CanGoBack)
            {
                containerView.GoBack();
                if (containerView.BackStackDepth == 0)
                {
                    if (containerView.ContentTransitions.Any(ct
                            => ((ct as NavigationThemeTransition)?.DefaultNavigationTransitionInfo as
                                   SlideNavigationTransitionInfo)?.Effect ==
                               SlideNavigationTransitionEffect.FromLeft))
                    {
                        containerView.HorizontalAlignment = HorizontalAlignment.Right;
                    }

                    containerView.HorizontalAlignment = HorizontalAlignment.Left;
                }

                windowInformation.UnregisterSubViewModel(request.ViewModel);
                return ValueTask.FromResult(true);
            }


            return ClosePage(request.ViewModel, attribute);
        }
    }
}
