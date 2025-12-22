namespace MvvmCross.Plugin.Color.Platforms.Android
{
    using Nivaes.App.Cross;

    [Preserve(AllMembers = true)]
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
