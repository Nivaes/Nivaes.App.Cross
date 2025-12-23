namespace Nivaes.App.Cross.Droid
{
    using Android.Views;

    public static class MvxBindingContextOwnerExtensions
    {
        extension(IMvxBindingContextOwner owner)
        {
            public View? BindingInflate(int resourceId, ViewGroup? viewGroup)
            {
                var context = (IMvxAndroidBindingContext?)owner.BindingContext;
                return context?.BindingInflate(resourceId, viewGroup);
            }

            public View? BindingInflate(int resourceId, ViewGroup viewGroup, bool attachToParent)
            {
                var context = (IMvxAndroidBindingContext?)owner.BindingContext;
                return context?.BindingInflate(resourceId, viewGroup, attachToParent);
            }
        }
    }
}
