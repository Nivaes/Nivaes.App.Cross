namespace MvvmCross.Platforms.Tvos.Binding.Target
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;
    using Nivaes.App.Cross;

    public class MvxUIButtonTitleTargetBinding : CrossConvertingTargetBinding
    {
        private readonly UIControlState _state;

        protected UIButton Button => Target as UIButton;

        public MvxUIButtonTitleTargetBinding(UIButton button, UIControlState state = UIControlState.Normal)
            : base(button)
        {
            _state = state;
            if (button == null)
            {
                MvxBindingLog.Instance?.LogError("UIButton is null in MvxUIButtonTitleTargetBinding");
            }
        }

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(string);

        protected override void SetValueImpl(object target, object value)
        {
            ((UIButton)target).SetTitle(value as string, _state);
        }
    }
}
