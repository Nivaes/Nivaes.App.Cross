namespace Nivaes.App.Cross.Droid
{
    using Android.Content;
    using Android.Util;
    using Android.Views;

    public interface IMvxAndroidViewFactory
    {
        View? CreateView(View? parent, string name, Context context, IAttributeSet attrs);
    }
}
