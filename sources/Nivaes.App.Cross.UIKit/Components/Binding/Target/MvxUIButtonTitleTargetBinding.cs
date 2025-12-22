namespace MvvmCross.Platforms.Ios.Binding.Target
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Binding;
    using Nivaes.App.Cross;

    public class MvxUIButtonTitleTargetBinding(UIButton button, UIControlState state = UIControlState.Normal)
       : CrossConvertingTargetBinding(button)
    {
        protected UIButton? Button => Target as UIButton;

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(string);

        protected override void SetValueImpl(object target, object? value)
        {
            ((UIButton)target).SetTitle(value as string, state);
        }
    }
}