namespace Nivaes.App.Cross.Droid
{
    [Obsolete("", true)]
    public static class MvxColorExtensions
    {
        private static readonly MvxAndroidColor _mvxNativeColor = new MvxAndroidColor();

        public static global::Android.Graphics.Color ToNativeColor(this System.Drawing.Color color)
        {
            return _mvxNativeColor.ToNativeColor(color);
        }
    }
}
