namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Android.Text;
    using MvvmCross.Binding;
    using MvvmCross.Binding.Extensions;

    public class MvxTextViewTextFormattedTargetBinding(TextView target)
    : MvxAndroidTargetBinding(target), ICrossEditableTextView
    {
        private readonly bool _isEditTextBinding = target is EditText;
        private CrossAndroidTargetEventSubscription<TextView, AfterTextChangedEventArgs>? _subscription;

        protected TextView? TextView => Target as TextView;

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(ISpanned);

        [RequiresUnreferencedCode("This method uses reflection to get type information and perform conversions which may not be preserved by trimming.")]
        protected override bool ShouldSkipSetValueForViewSpecificReasons(object target, object? value)
        {
            if (!_isEditTextBinding)
                return false;

            return this.ShouldSkipSetValueAsHaveNearlyIdenticalNumericText(target, value);
        }

        protected override void SetValueImpl(object target, object? value)
        {
            ((TextView)target).TextFormatted = (ISpanned?)value;
        }

        public override CrossBindingMode DefaultMode => _isEditTextBinding ? CrossBindingMode.TwoWay : CrossBindingMode.OneWay;

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            var view = TextView;
            if (view == null)
                return;

            _subscription = view.DroidWeakSubscribe<TextView, AfterTextChangedEventArgs>(
                nameof(view.AfterTextChanged),
                EditTextOnAfterTextChanged);
        }

        private void EditTextOnAfterTextChanged(object? sender, AfterTextChangedEventArgs afterTextChangedEventArgs)
        {
            FireValueChanged(TextView?.TextFormatted);
        }

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

        public string? CurrentText
        {
            get
            {
                var view = TextView;
                return view?.TextFormatted?.ToString();
            }
        }
    }
}