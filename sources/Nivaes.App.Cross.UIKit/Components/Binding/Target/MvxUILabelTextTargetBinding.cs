namespace MvvmCross.Platforms.Ios.Binding.Target
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Binding;
    using Nivaes.App.Cross;

    public class MvxUILabelTextTargetBinding(UILabel target)
        : CrossConvertingTargetBinding(target)
    {
        protected UILabel? View => Target as UILabel;

        public override MvxBindingMode DefaultMode => MvvmCross.Binding.MvxBindingMode.OneWay;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(string);

        protected override void SetValueImpl(object target, object? value)
        {
            var view = (UILabel?)target;
            if (view == null)
                return;

            view.Text = (string?)value;
        }
    }
}