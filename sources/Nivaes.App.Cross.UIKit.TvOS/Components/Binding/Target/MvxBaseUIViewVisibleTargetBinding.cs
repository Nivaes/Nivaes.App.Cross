namespace MvvmCross.Platforms.Tvos.Binding.Target
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Binding;
    using Nivaes.App.Cross;

    public abstract class MvxBaseUIViewVisibleTargetBinding 
        : CrossConvertingTargetBinding
    {
        protected UIView View => (UIView)Target;

        protected MvxBaseUIViewVisibleTargetBinding(UIView target)
            : base(target)
        {
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(bool);
    }
}
