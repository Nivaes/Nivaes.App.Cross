namespace Nivaes.App.Cross.AppKitOS
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;

    public class MvxNSButtonTitleTargetBinding 
        : MvxMacTargetBinding
    {
        protected NSButton? Button
        {
            get { return base.Target as NSButton; }
        }

        public MvxNSButtonTitleTargetBinding(NSButton button)
            : base(button)
        {
            if (button == null)
            {
                CrossBindingLog.Instance?.LogError("NSButton is null in MvxNSButtonTitleTargetBinding");
            }
        }

        public override CrossBindingMode DefaultMode
        {
            get { return CrossBindingMode.OneWay; }
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType
        {
            get { return typeof(string); }
        }

        protected override void SetValueImpl(object target, object? value)
        {
            var button = this.Button;
            if (button == null)
                return;

            button.Title = value as string;
        }
    }
}
