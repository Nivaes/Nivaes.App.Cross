using Microsoft.Extensions.Logging;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media.Animation; 

namespace Nivaes.App.Cross.WinUI
{
    public sealed class RegionWinUIPressenterAction
        : PressenterAction<RegionPresentationAttribute>
    {
        #region Constructor
        public RegionWinUIPressenterAction(
                IPressenterActionContext context,
                ICrossWindowsFrame rootFrame,
                ILogger<RegionWinUIPressenterAction> logger)
            : base(context, rootFrame, logger)
        {
        }
        #endregion

        protected override ValueTask<bool> ShowAction(RegionPresentationAttribute attribute, IViewModelRequest request)
        {
            var windowInformation = GetWindowInformation(request);
          
            var containerView = windowInformation.RootFrame.UnderlyingControl.FindControl<Frame>(attribute.RegionName);         

            windowInformation.RegisterSubViewModel(request.ViewModel);

            if (containerView != null)
            {
                var requestBuffer = ViewModelRequestSerializer.Serializer(request);

                containerView.Navigate(attribute.ViewType, requestBuffer);

                containerView.HorizontalAlignment = HorizontalAlignment.Stretch;
                return ValueTask.FromResult(true);
            }
            else
            {
                //throw new AppException($"Not containerView found for {viewType.FullName}");
                Logger.LogError($"Not containerView found for {attribute.ViewType.FullName}");
                return ValueTask.FromResult(false);
            }            
        }

        protected override ValueTask<bool> CloseAction(ICrossViewModel viewModel, RegionPresentationAttribute attribute)
        {
            var windowInformation = GetWindowInformation(viewModel);

            var viewType = Singleton<ViewModelViewsKeyContainerManager>.Instance
               .GetValue(viewModel.GetType());


            var containerView = windowInformation.RootFrame.UnderlyingControl?.FindControl<Frame>(attribute.RegionName);

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


            return ClosePage(viewModel, attribute);
        }
    }
}
