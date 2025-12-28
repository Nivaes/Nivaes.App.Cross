namespace Nivaes.App.Cross.UIKitOS
{
    using UIKit;

    public class MvxUIViewVisibleTargetBinding(UIView target)
        : MvxBaseUIViewVisibleTargetBinding(target)
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