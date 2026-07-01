namespace MvvmCross.Platforms.Ios.Binding.Target
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    public class MvxUIViewVisibilityTargetBinding(UIView target)
        : CrossConvertingTargetBinding(target)
    {
        protected UIView? View => (UIView?)Target;

        public override CrossBindingMode DefaultMode => CrossBindingMode.OneWay;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(CrossVisibility);

        protected override void SetValueImpl(object target, object? value)
        {
            var view = (UIView)target;
            if (value is not CrossVisibility visibility)
            {
                CrossBindingLogger.Instance?.LogWarning("Visibility out of range {Value}", value);
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