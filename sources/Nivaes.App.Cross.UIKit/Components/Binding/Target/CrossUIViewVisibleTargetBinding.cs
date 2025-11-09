namespace Nivaes.App.Cross.UIKit
{
    using UIKit;

    public class CrossUIViewVisibleTargetBinding(UIView target)
        : CrossBaseUIViewVisibleTargetBinding(target)
    {
        protected override void SetValueImpl(object target, object? value)
        {
            var view = View;
            if (view == null) return;

            var visible = value.ConvertToBoolean();
            view.Hidden = !visible;
        }
    }
}