using Android.Views;

namespace Nivaes.App.Cross.Droid
{
    using MvvmCross.Binding.Extensions;

    public class MvxViewVisibleBinding(object target)
        : MvxBaseViewVisibleBinding(target)
    {
        protected override void SetValueImpl(object target, object? value)
        {
            ((View)target).Visibility = value.ConvertToBoolean() ? ViewStates.Visible : ViewStates.Gone;
        }
    }
}