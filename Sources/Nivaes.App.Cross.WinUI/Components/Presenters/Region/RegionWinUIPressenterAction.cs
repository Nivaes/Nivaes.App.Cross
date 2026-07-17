using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation;

namespace Nivaes.App.Cross.WinUI
{
    public sealed class RegionWinUIPressenterAction 
        : WinUIPressenterAction<RegionPresentationAttribute>
    {
        #region Constructor
        public RegionWinUIPressenterAction(
                IPressenterActionContext context,
                ICrossViewsContainer viewsContainer,
                ICrossWindowsFrame rootFrame,
                ICrossWindowsViewModelRequestTranslator requestTranslator,
                ILogger<RegionWinUIPressenterAction> logger)
            : base(context, viewsContainer, rootFrame, requestTranslator, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(Type viewType, RegionPresentationAttribute attribute, CrossViewModelRequest request)
        {
            if (viewType.HasRegionAttribute())
            {
                var windowInformation = GetWindowInformation(request);
                var requestText = GetRequestText(request);
                var containerView =
                    windowInformation.RootFrame.UnderlyingControl.FindControl<Frame>(viewType.GetRegionName());
                if (request is CrossViewModelInstanceRequestWithSource targetRequest &&
                    targetRequest.ViewModelInstance != null)
                {
                    windowInformation.RegisterSubViewModel(targetRequest.ViewModelInstance);
                }

                if (containerView != null)
                {
                    containerView.Navigate(viewType, requestText);

                    containerView.HorizontalAlignment = HorizontalAlignment.Stretch;
                    return ValueTask.FromResult(true);
                }
            }

            return ValueTask.FromResult(true);
        }

        protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, RegionPresentationAttribute attribute)
        {
            var windowInformation = GetWindowInformation(viewModel);

            if (base.ViewsContainer == null)
            {
                return ValueTask.FromResult(false);
            }

            var viewType = base.ViewsContainer.GetViewType(viewModel.GetType());
            if (viewType.HasRegionAttribute())
            {
                var containerView =
                    windowInformation.RootFrame.UnderlyingControl?.FindControl<Frame>(viewType.GetRegionName());

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

                    windowInformation.UnregisterSubViewModel(viewModel);
                    return ValueTask.FromResult(true);
                }
            }

            return ClosePage(viewModel, attribute);
        }
    }
}
