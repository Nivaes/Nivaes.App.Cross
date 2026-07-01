namespace Nivaes.App.Cross.UIKitOS
{
    using System.Diagnostics.CodeAnalysis;
    using Nivaes.App.Cross;

    public class MvxUILabelTextTargetBinding(UILabel target)
        : CrossConvertingTargetBinding(target)
    {
        protected UILabel? View => Target as UILabel;

        public override CrossBindingMode DefaultMode => CrossBindingMode.OneWay;

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