namespace Nivaes.App.Cross.Droid
{
    using Android.Views;

    public interface ICrossAndroidBindingContext
        : ICrossBindingContext
    {
        ICrossLayoutInflaterHolder LayoutInflaterHolder { get; set; }

        View BindingInflate(int resourceId, ViewGroup viewGroup);

        View BindingInflate(int resourceId, ViewGroup viewGroup, bool attachToParent);
    }
}
