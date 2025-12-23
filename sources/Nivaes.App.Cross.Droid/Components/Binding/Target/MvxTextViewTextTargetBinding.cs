using Android.Text;

namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;

    using MvvmCross.Binding;
    using MvvmCross.Binding.Extensions;

    public class MvxTextViewTextTargetBinding
    : MvxAndroidTargetBinding
        , ICrossEditableTextView
    {
        private readonly bool _isEditTextBinding;
        private CrossAndroidTargetEventSubscription<TextView, AfterTextChangedEventArgs>? _subscription;

        protected TextView? TextView => Target as TextView;

        public MvxTextViewTextTargetBinding(TextView target)
            : base(target)
        {
            _isEditTextBinding = target is EditText;
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(string);

        [RequiresUnreferencedCode("This method uses reflection to get type information and perform conversions which may not be preserved by trimming.")]
        protected override bool ShouldSkipSetValueForViewSpecificReasons(object target, object? value)
        {
            if (!_isEditTextBinding)
                return false;

            return this.ShouldSkipSetValueAsHaveNearlyIdenticalNumericText(target, value);
        }

        protected override void SetValueImpl(object target, object? value)
        {
            ((TextView)target).SetText((string?)value, TextView.BufferType.Normal);
        }

        public override CrossBindingMode DefaultMode => _isEditTextBinding ? CrossBindingMode.TwoWay : CrossBindingMode.OneWay;

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            if (_isEditTextBinding)
            {
                var view = TextView;
                if (view == null)
                    return;

                _subscription = view.DroidWeakSubscribe<TextView, AfterTextChangedEventArgs>(
                    nameof(view.AfterTextChanged),
                    EditTextOnAfterTextChanged);
            }
        }

        private void EditTextOnAfterTextChanged(object? sender, AfterTextChangedEventArgs e)
        {
            FireValueChanged(TextView?.Text);
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
                return view?.Text;
            }
        }
    }
}