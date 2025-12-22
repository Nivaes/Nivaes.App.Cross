namespace MvvmCross.Platforms.Tvos.Binding.Target
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;
    using Nivaes.App.Cross;

    public class MvxUIViewVisibilityTargetBinding : CrossConvertingTargetBinding
    {
        protected UIView View => (UIView)Target;

        public MvxUIViewVisibilityTargetBinding(UIView target)
            : base(target)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(CrossVisibility);

        protected override void SetValueImpl(object target, object value)
        {
            var view = (UIView)target;
            var visibility = (CrossVisibility)value;
            switch (visibility)
            {
                case CrossVisibility.Visible:
                    view.Hidden = false;
                    break;

                case CrossVisibility.Collapsed:
                    view.Hidden = true;
                    break;

                default:
                    MvxBindingLog.Instance?.LogWarning("Visibility out of range {Value}", value);
                    break;
            }
        }
    }
}
