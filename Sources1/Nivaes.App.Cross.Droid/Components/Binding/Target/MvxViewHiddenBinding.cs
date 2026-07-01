using Android.Views;

namespace Nivaes.App.Cross.Droid
{
    public class MvxViewHiddenBinding(View target)
    : MvxBaseViewVisibleBinding(target)
    {
        protected override void SetValueImpl(object target, object? value)
        {
            ((View)target).Visibility = value.ConvertToBoolean() ? ViewStates.Gone : ViewStates.Visible;
        }
    }
}