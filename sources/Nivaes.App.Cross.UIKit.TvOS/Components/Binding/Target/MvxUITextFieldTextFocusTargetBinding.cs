namespace MvvmCross.Platforms.Tvos.Binding.Target
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Binding;
    using Nivaes.App.Cross;
    using UIKit;

    public class MvxUITextFieldTextFocusTargetBinding 
        : CrossTargetBinding
    {
        private bool _subscribed;

        protected UITextField TextField => Target as UITextField;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(string);

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        public MvxUITextFieldTextFocusTargetBinding(object target)
            : base(target)
        {
        }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("This method may perform type conversions which may not be preserved by trimming")]
        public override void SetValue(object value)
        {
            if (TextField == null) return;

            value = value ?? string.Empty;
            TextField.Text = value.ToString();
        }

        [System.Diagnostics.CodeAnalysis.RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            if (TextField == null) return;

            TextField.EditingDidEnd += HandleLostFocus;
            _subscribed = true;
        }

        private void HandleLostFocus(object sender, EventArgs e)
        {
            if (TextField == null) return;

            FireValueChanged(TextField.Text);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (_subscribed)
                {
                    var textField = TextField;
                    if (textField != null)
                    {
                        textField.EditingDidEnd -= HandleLostFocus;
                    }
                }
            }
            base.Dispose(isDisposing);
        }
    }
}
