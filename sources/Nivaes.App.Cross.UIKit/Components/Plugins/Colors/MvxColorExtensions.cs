namespace Nivaes.App.Cross.UIKitOS
{
    public static class MvxColorExtensions
    {
        public static UIColor ToNativeColor(this System.Drawing.Color color)
        {
            return MvxIosColor.ToUIColor(color);
        }
    }
}
