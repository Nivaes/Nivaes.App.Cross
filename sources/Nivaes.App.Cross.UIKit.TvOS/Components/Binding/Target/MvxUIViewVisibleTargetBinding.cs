namespace MvvmCross.Platforms.Tvos.Binding.Target
{
    using MvvmCross.Binding.Extensions;
    using UIKit;

    public class MvxUIViewVisibleTargetBinding
        : MvxBaseUIViewVisibleTargetBinding
    {
        public MvxUIViewVisibleTargetBinding(UIView target)
            : base(target)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = View;
            if (view == null)
                return;

            var visible = value.ConvertToBoolean();
            view.Hidden = !visible;
        }
    }
}
