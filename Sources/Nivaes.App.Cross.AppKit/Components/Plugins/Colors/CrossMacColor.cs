namespace Nivaes.App.Cross.AppKitLib
{
    public class CrossMacColor : ICrossNativeColor
    {
        public object ToNative(System.Drawing.Color color)
        {
            return ToUIColor(color);
        }

        public static NSColor ToUIColor(System.Drawing.Color color)
        {
            return NSColor.FromDeviceRgba(color.R / 255.0f, color.G / 255.0f, color.B / 255.0f, color.A / 255.0f);
        }
    }
}
