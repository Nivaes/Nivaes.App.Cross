namespace Nivaes.App.Cross.Droid
{
    using Android.Views;

    public class CrossViewHiddenBinding(View target)
        : CrossBaseViewVisibleBinding(target)
    {
        protected override void SetValueImpl(object target, object? value)
        {
            ((View)target).Visibility = value.ConvertToBoolean() ? ViewStates.Gone : ViewStates.Visible;
        }
    }
}