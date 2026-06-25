namespace Nivaes.App.Cross.WinUI
{
    public static class MvxStoreColorExtensions
    {
        public static Windows.UI.Color ToNativeColor(this System.Drawing.Color color)
        {
            var windowsColor = Windows.UI.Color.FromArgb(color.A, color.R, color.G, color.B);
            return windowsColor;
        }
    }
}
