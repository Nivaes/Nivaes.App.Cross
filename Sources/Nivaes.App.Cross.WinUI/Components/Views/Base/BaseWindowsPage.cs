using Microsoft.UI;
using Windows.ApplicationModel.Core;
using Windows.Foundation.Metadata;
using Windows.UI.ViewManagement;

namespace Nivaes.App.Cross.WinUI
{
    public abstract class BaseWindowsPage<TViewModel>
        : CrossWindowsPage<TViewModel>
        where TViewModel : ICrossViewModel
    {
        protected virtual bool IsTransparentBar => false;
        protected virtual bool CanBackStack => true;

        protected BaseWindowsPage()
        {
            base.Loading += (o, e) =>
            {
                if (!CanBackStack)
                {
                    base.ClearBackStack();
                }
            };

            SetColors();
        }

        #region Colors
        private void SetColors()
        {
            if (IsTransparentBar)
            {
                TransparentColors();
            }
            else
            {
                NormalColor();
            }
        }

        private void NormalColor()
        {
            try
            {
                CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = false;

                if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView") &&
                     ApiInformation.IsMethodPresent("Windows.UI.ViewManagement.ApplicationView", nameof(ApplicationView.GetForCurrentView)))
                {
                    var titleBar = ApplicationView.GetForCurrentView().TitleBar;

                    if (titleBar != null)
                    {
                        titleBar.ButtonBackgroundColor = null;
                        //titleBar.ButtonInactiveBackgroundColor = null;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private async void TransparentColors()
        {
            try
            {
                CoreApplication.GetCurrentView().TitleBar.ExtendViewIntoTitleBar = true;

                if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.ApplicationView") &&
                     ApiInformation.IsMethodPresent("Windows.UI.ViewManagement.ApplicationView", nameof(ApplicationView.GetForCurrentView)))
                {
                    var titleBar = ApplicationView.GetForCurrentView().TitleBar;

                    if (titleBar != null)
                    {
                        titleBar.ButtonBackgroundColor = Colors.Transparent;
                        //titleBar.ButtonInactiveBackgroundColor = Colors.Transparent;
                    }
                }
            }
            catch (Exception ex)
            {
            }



            //if (ApiInformation.IsTypePresent("Windows.UI.ViewManagement.StatusBar") &&
            //       ApiInformation.IsMethodPresent("Windows.UI.ViewManagement.StatusBar", nameof(StatusBar.ShowAsync)))
            //{
            //    StatusBar statusBar = StatusBar.GetForCurrentView();

            //    if (statusBar != null)
            //    {
            //        await statusBar.ShowAsync();
            //        statusBar.BackgroundColor = Colors.LightGray;
            //        statusBar.BackgroundOpacity = 0.8;
            //        statusBar.ForegroundColor = Colors.DarkGray;
            //    }
            //}
        }
        #endregion
    }
}
