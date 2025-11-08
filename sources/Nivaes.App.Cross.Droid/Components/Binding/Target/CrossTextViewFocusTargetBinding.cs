namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Android.Views;

    public class CrossTextViewFocusTargetBinding
        : CrossAndroidTargetBinding
    {
        private CrossAndroidTargetEventSubscription<View, View.FocusChangeEventArgs>? _subscription;

        protected EditText? TextField => Target as EditText;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(string);

        public override CrossBindingMode DefaultMode => CrossBindingMode.TwoWay;

        public CrossTextViewFocusTargetBinding(object target)
            : base(target)
        {
        }

        protected override void SetValueImpl(object target, object? value)
        {
            if (TextField == null) return;

            value = value ?? string.Empty;
            TextField.Text = value.ToString();
        }

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            if (TextField == null) return;

            _subscription = TextField.WeakSubscribe<View, View.FocusChangeEventArgs>(
                nameof(TextField.FocusChange),
                HandleLostFocus);
        }

        private void HandleLostFocus(object? sender, View.FocusChangeEventArgs e)
        {
            if (TextField == null) return;

            if (!e.HasFocus)
                FireValueChanged(TextField.Text);
        }

        [RequiresUnreferencedCode("This method calls SubscribeToEvents which may use reflection to subscribe to events which may not be preserved by trimming")]
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _subscription?.Dispose();
                _subscription = null;
            }
            base.Dispose(isDisposing);
        }
    }
}