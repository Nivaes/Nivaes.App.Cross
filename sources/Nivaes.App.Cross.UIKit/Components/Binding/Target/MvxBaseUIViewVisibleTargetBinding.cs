namespace MvvmCross.Platforms.Ios.Binding.Target
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Binding;
    using Nivaes.App.Cross;

    public abstract class MvxBaseUIViewVisibleTargetBinding(UIView target)
        : CrossConvertingTargetBinding(target)
    {
        protected UIView? View => (UIView?)Target;

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(bool);
    }
}