namespace Nivaes.App.Cross.UIKitLib
{
    public static class MvxColorExtensions
    {
        public static UIColor ToNativeColor(this System.Drawing.Color color)
        {
            return MvxIosColor.ToUIColor(color);
        }
    }
}
