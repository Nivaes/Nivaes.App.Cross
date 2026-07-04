namespace Nivaes.App.Cross.AppKitLib
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public class MvxNSViewVisibleTargetBinding
        : MvxMacTargetBinding
    {
        protected NSView? View
        {
            get { return (NSView?)Target; }
        }

        public MvxNSViewVisibleTargetBinding(NSView target)
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
            get { return typeof(bool); }
        }

        protected override void SetValueImpl(object target, object? value)
        {
            var view = this.View;
            if (view == null)
                return;

            var visible = (bool?)value;
            view.Hidden = !visible ?? false;
        }
    }
}
