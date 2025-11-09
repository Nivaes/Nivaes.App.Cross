namespace Nivaes.App.Cross.UIKit
{
    using System.Diagnostics.CodeAnalysis;

    public abstract class CrossBaseUIViewVisibleTargetBinding(UIView target)
        : CrossConvertingTargetBinding(target)
    {
        protected UIView? View => (UIView?)Target;

        public override CrossBindingMode DefaultMode => CrossBindingMode.OneWay;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(bool);
    }
}