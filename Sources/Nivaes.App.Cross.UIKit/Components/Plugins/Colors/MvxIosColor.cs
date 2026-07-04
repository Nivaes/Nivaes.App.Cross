namespace Nivaes.App.Cross.UIKitLib
{
    public class MvxIosColor : ICrossNativeColor
    {
        public object ToNative(System.Drawing.Color color)
        {
            return ToUIColor(color);
        }

        public static UIColor ToUIColor(System.Drawing.Color color)
        {
            return new UIColor(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
        }
    }
}
