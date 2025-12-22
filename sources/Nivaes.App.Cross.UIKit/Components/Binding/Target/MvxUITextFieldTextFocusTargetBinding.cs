namespace MvvmCross.Platforms.Ios.Binding.Target
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Binding;
    using MvvmCross.Binding.Bindings.Target;
    using Nivaes.App.Cross;

    public class MvxUITextFieldTextFocusTargetBinding(UITextField target)
        : MvxTargetBinding(target)
    {
        private CrossWeakEventSubscription<UITextField>? _subscription;

        protected UITextField? TextField => Target as UITextField;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(string);

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        [RequiresUnreferencedCode("This method may perform type conversions which may not be preserved by trimming")]
        public override void SetValue(object? value)
        {
            if (TextField == null) return;

            value ??= string.Empty;
            TextField.Text = value.ToString();
        }

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            var textField = TextField;
            if (textField == null)
                return;

            _subscription = textField.WeakSubscribe(nameof(textField.EditingDidEnd), HandleLostFocus);
        }

        private void HandleLostFocus(object? sender, EventArgs e)
        {
            var textField = TextField;
            if (textField == null) return;

            FireValueChanged(textField.Text);
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing) return;

            _subscription?.Dispose();
            _subscription = null;
        }
    }
}