namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;

    public class CrossAppCompatSpinnerSelectedItemBinding
        : CrossAndroidTargetBinding
    {
        private object? _currentValue;
        private CrossAndroidTargetEventSubscription<CrossAppCompatSpinner, AdapterView.ItemSelectedEventArgs>? _subscription;

        protected CrossAppCompatSpinner? Spinner => (CrossAppCompatSpinner?)Target;

        public CrossAppCompatSpinnerSelectedItemBinding(CrossAppCompatSpinner spinner)
            : base(spinner)
        {
        }

        private void SpinnerItemSelected(object? sender, AdapterView.ItemSelectedEventArgs e)
        {
            var spinner = Spinner;
            if (spinner == null)
                return;

            var newValue = spinner.Adapter.GetRawItem(e.Position);

            bool changed;
            if (newValue == null)
            {
                changed = _currentValue != null;
            }
            else
            {
                changed = !newValue.Equals(_currentValue);
            }

            if (!changed)
            {
                return;
            }

            _currentValue = newValue;
            FireValueChanged(newValue);
        }

        protected override void SetValueImpl(object target, object? value)
        {
            var spinner = (CrossAppCompatSpinner)target;

            if (value == null)
            {
                CrossBindingLog.Instance?.LogWarning(
                    "Null values not permitted in spinner SelectedItem binding currently");
                return;
            }

            if (!value.Equals(_currentValue))
            {
                var index = spinner.Adapter.GetPosition(value);
                if (index < 0)
                {
                    CrossBindingLog.Instance?.LogWarning("Value not found for spinner @{Value}", value);
                    return;
                }
                _currentValue = value;
                spinner.SetSelection(index);
            }
        }

        public override CrossBindingMode DefaultMode => CrossBindingMode.TwoWay;

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            var spinner = Spinner;
            if (spinner == null)
                return;

            _subscription = spinner.WeakSubscribe<CrossAppCompatSpinner, AdapterView.ItemSelectedEventArgs>(
                nameof(spinner.ItemSelected),
                SpinnerItemSelected);
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(object);

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
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