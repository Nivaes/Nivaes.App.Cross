namespace MvvmCross.Platforms.Ios.Binding.Target
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;
    using MvvmCross.Binding.Bindings.Target;
    using Nivaes.App.Cross;

    public class MvxUIViewVisibilityTargetBinding(UIView target)
        : MvxConvertingTargetBinding(target)
    {
        protected UIView? View => (UIView?)Target;

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(CrossVisibility);

        protected override void SetValueImpl(object target, object? value)
        {
            var view = (UIView)target;
            if (value is not CrossVisibility visibility)
            {
                MvxBindingLog.Instance?.LogWarning("Visibility out of range {Value}", value);
                return;
            }

            view.Hidden = visibility switch
            {
                CrossVisibility.Visible => false,
                CrossVisibility.Collapsed => true,
                _ => view.Hidden
            };
        }
    }
}