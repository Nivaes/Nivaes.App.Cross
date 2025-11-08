namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

    public class CrossToolbarSubtitleBinding(Toolbar toolbar)
        : CrossConvertingTargetBinding(toolbar)
    {
        protected Toolbar? Toolbar => Target as Toolbar;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(string);

        public override CrossBindingMode DefaultMode => CrossBindingMode.OneWay;

        protected override void SetValueImpl(object target, object? value)
        {
            if (target is Toolbar view)
                view.Subtitle = (string?)value;
        }
    }
}