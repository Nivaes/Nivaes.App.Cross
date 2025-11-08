namespace Nivaes.App.Cross.Droid
{
    using Android.Views;

    public class CrossViewVisibleBinding(object target)
        : CrossBaseViewVisibleBinding(target)
    {
        protected override void SetValueImpl(object target, object? value)
        {
            ((View)target).Visibility = value.ConvertToBoolean() ? ViewStates.Visible : ViewStates.Gone;
        }
    }
}