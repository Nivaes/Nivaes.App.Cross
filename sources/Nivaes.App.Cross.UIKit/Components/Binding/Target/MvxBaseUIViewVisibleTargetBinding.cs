namespace Nivaes.App.Cross.UIKit
{
    using System.Diagnostics.CodeAnalysis;
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