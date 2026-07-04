namespace Nivaes.App.Cross.AppKitLib
{
    public static class MvxColorExtensions
    {
        public static NSColor ToNativeColor(this System.Drawing.Color color)
        {
            return CrossMacColor.ToUIColor(color);
        }
    }
}
