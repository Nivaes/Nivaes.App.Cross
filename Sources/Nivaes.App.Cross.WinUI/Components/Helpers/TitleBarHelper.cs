using Microsoft.UI;
using Microsoft.UI.Xaml;
using Windows.UI;

namespace Nivaes.App.Cross.WinUI
{
    internal static class TitleBarHelper
    {
        public static void ApplySystemThemeToCaptionButtons(Window window, ElementTheme currentTheme)
        {
            if (window.AppWindow != null)
            {
                var foregroundColor = currentTheme == ElementTheme.Dark ? Colors.White : Colors.Black;
                window.AppWindow.TitleBar.ButtonForegroundColor = foregroundColor;
                window.AppWindow.TitleBar.ButtonHoverForegroundColor = foregroundColor;

                var backgroundHoverColor = currentTheme == ElementTheme.Dark ? Color.FromArgb(24, 255, 255, 255) : Windows.UI.Color.FromArgb(24, 0, 0, 0);
                window.AppWindow.TitleBar.ButtonHoverBackgroundColor = backgroundHoverColor;
            }
        }
    }
}
