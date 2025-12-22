namespace MvvmCross.Platforms.Tvos.Binding.Target
{
    using MvvmCross.Binding.Extensions;
    using UIKit;

    public class MvxUIViewHiddenTargetBinding : MvxBaseUIViewVisibleTargetBinding
    {
        public MvxUIViewHiddenTargetBinding(UIView target)
            : base(target)
        {
        }

        protected override void SetValueImpl(object target, object value)
        {
            var view = View;
            if (view == null)
                return;

            var hidden = value.ConvertToBoolean();
            view.Hidden = hidden;
        }
    }
}
