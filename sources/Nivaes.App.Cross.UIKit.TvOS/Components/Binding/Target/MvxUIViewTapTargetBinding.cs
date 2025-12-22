namespace MvvmCross.Platforms.Tvos.Binding.Target
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Windows.Input;
    using MvvmCross.Binding;
    using MvvmCross.Platforms.Tvos.Binding.Views.Gestures;
    using Nivaes.App.Cross;
    using UIKit;


    public class MvxUIViewTapTargetBinding
        : CrossConvertingTargetBinding
    {
        private readonly MvxTapGestureRecognizerBehaviour _behaviour;

        public MvxUIViewTapTargetBinding(UIView target, uint numberOfTapsRequired = 1, uint numberOfTouchesRequired = 1, bool cancelsTouchesInView = true)
            : base(target)
        {
            _behaviour = new MvxTapGestureRecognizerBehaviour(target, numberOfTapsRequired, numberOfTouchesRequired, cancelsTouchesInView);
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(ICommand);

        protected override void SetValueImpl(object target, object value)
        {
            _behaviour.Command = (ICommand)value;
        }
    }
}
