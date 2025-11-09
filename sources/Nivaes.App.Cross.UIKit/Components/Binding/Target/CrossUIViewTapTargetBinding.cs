namespace Nivaes.App.Cross.UIKit
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Input;

    public class CrossUIViewTapTargetBinding(
            UIView target,
            uint numberOfTapsRequired = 1,
            uint numberOfTouchesRequired = 1,
            bool cancelsTouchesInView = true)
        : CrossConvertingTargetBinding(target)
    {
        private readonly CrossTapGestureRecognizerBehaviour _behaviour = new(target, numberOfTapsRequired,
            numberOfTouchesRequired, cancelsTouchesInView);

        public override CrossBindingMode DefaultMode => CrossBindingMode.OneWay;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(ICommand);

        protected override void SetValueImpl(object target, object? value)
        {
            _behaviour.Command = (ICommand?)value;
        }
    }
}