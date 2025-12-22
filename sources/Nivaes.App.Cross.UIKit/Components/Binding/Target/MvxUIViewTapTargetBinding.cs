namespace MvvmCross.Platforms.Ios.Binding.Target
{
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Input;
    using MvvmCross.Binding;
    using MvvmCross.Platforms.Ios.Binding.Views.Gestures;
    using Nivaes.App.Cross;

    public class MvxUIViewTapTargetBinding(
        UIView target,
        uint numberOfTapsRequired = 1,
        uint numberOfTouchesRequired = 1,
        bool cancelsTouchesInView = true)
    : CrossConvertingTargetBinding(target)
    {
        private readonly MvxTapGestureRecognizerBehaviour _behaviour = new(target, numberOfTapsRequired,
            numberOfTouchesRequired, cancelsTouchesInView);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(ICommand);

        protected override void SetValueImpl(object target, object? value)
        {
            _behaviour.Command = (ICommand?)value;
        }
    }
}