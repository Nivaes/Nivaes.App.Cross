using Android.Views;

namespace Nivaes.App.Cross.Droid
{
    public interface IMvxAndroidBindingContext
        : ICrossBindingContext
    {
        IMvxLayoutInflaterHolder LayoutInflaterHolder { get; set; }

        View? BindingInflate(int resourceId, ViewGroup? viewGroup);

        View? BindingInflate(int resourceId, ViewGroup? viewGroup, bool attachToParent);
    }
}
