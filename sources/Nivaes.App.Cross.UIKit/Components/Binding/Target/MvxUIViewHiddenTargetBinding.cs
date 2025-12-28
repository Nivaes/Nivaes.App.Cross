namespace Nivaes.App.Cross.UIKitOS
{
    using UIKit;

    public class MvxUIViewHiddenTargetBinding(UIView target)
    : MvxBaseUIViewVisibleTargetBinding(target)
    {
        protected override void SetValueImpl(object target, object? value)
        {
            var view = View;
            if (view == null) return;

            var hidden = value.ConvertToBoolean();
            view.Hidden = hidden;
        }
    }
}