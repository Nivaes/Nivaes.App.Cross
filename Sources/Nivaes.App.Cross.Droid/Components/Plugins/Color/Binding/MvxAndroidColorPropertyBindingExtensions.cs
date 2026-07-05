using Android.Views;

namespace Nivaes.App.Cross.Droid
{
    public static class MvxAndroidColorPropertyBindingExtensions
    {
        public static string BindBackgroundColor(this View view)
           => MvxAndroidColorPropertyBinding.View_BackgroundColor;

        public static string BindTextColor(this TextView view)
           => MvxAndroidColorPropertyBinding.TextView_TextColor;
    }
}
