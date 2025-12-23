namespace Nivaes.App.Cross.UIKit
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