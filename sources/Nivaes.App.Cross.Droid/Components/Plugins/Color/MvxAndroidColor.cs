namespace Nivaes.App.Cross.Droid
{
    using Nivaes.App.Cross;

    public class MvxAndroidColor 
        : ICrossNativeColor
    {
        public object ToNative(System.Drawing.Color color)
        {
            return ToNativeColor(color);
        }

        public global::Android.Graphics.Color ToNativeColor(System.Drawing.Color color)
        {
            return new global::Android.Graphics.Color(color.R, color.G, color.B, color.A);
        }
    }
}
