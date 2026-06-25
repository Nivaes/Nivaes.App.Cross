using Microsoft.UI.Xaml.Media;

namespace Nivaes.App.Cross.WinUI
{
    public class CrossWinUIColor : ICrossNativeColor
    {
        public object ToNative(System.Drawing.Color color)
        {
            var nativeColor = color.ToNativeColor();
            return new SolidColorBrush(nativeColor);
        }
    }
}
