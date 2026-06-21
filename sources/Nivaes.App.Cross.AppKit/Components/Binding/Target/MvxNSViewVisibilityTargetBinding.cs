namespace Nivaes.App.Cross.AppKitOS
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;

    public class MvxNSViewVisibilityTargetBinding
        : MvxMacTargetBinding
    {
        protected NSView? View
        {
            get { return (NSView?)Target; }
        }

        public MvxNSViewVisibilityTargetBinding(NSView target)
            : base(target)
        {
        }

        public override CrossBindingMode DefaultMode
        {
            get { return CrossBindingMode.OneWay; }
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType
        {
            get { return typeof(CrossVisibility); }
        }

        protected override void SetValueImpl(object target, object? value)
        {
            var view = this.View;
            if (view == null)
                return;

            var visibility = (CrossVisibility?)value;
            switch (visibility)
            {
                case CrossVisibility.Visible:
                    view.Hidden = false;
                    break;

                case CrossVisibility.Collapsed:
                    view.Hidden = true;
                    break;

                default:
                    CrossBindingLogger.Instance?.LogWarning("Visibility out of range {Value}", value);
                    break;
            }
        }
    }
}
