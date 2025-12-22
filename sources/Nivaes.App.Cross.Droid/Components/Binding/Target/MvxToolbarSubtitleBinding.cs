namespace MvvmCross.Platforms.Android.Binding.Target
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Binding;
    using Nivaes.App.Cross;
    using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

    public class MvxToolbarSubtitleBinding(Toolbar toolbar)
        : CrossConvertingTargetBinding(toolbar)
    {
        protected Toolbar? Toolbar => Target as Toolbar;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(string);

        public override MvxBindingMode DefaultMode => MvxBindingMode.OneWay;

        protected override void SetValueImpl(object target, object? value)
        {
            if (target is Toolbar view)
                view.Subtitle = (string?)value;
        }
    }
}