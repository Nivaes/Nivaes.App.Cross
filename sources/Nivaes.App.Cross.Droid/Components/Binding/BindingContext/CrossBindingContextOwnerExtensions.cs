namespace Nivaes.App.Cross.Droid
{
    using Android.Views;

    public static class CrossBindingContextOwnerExtensions
    {
        public static View BindingInflate(this ICrossBindingContextOwner owner, int resourceId, ViewGroup viewGroup)
        {
            var context = (ICrossAndroidBindingContext)owner.BindingContext;
            return context.BindingInflate(resourceId, viewGroup);
        }

        public static View BindingInflate(this ICrossBindingContextOwner owner, int resourceId, ViewGroup viewGroup, bool attachToParent)
        {
            var context = (ICrossAndroidBindingContext)owner.BindingContext;
            return context.BindingInflate(resourceId, viewGroup, attachToParent);
        }
    }
}
