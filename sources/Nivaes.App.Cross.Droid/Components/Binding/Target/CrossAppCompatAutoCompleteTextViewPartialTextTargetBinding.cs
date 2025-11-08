namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.Logging;

    public class CrossAppCompatAutoCompleteTextViewPartialTextTargetBinding
        : MvxAndroidPropertyInfoTargetBinding<CrossAppCompatAutoCompleteTextView>
    {
        private CrossJavaEventSubscription<CrossAppCompatAutoCompleteTextView>? _subscription;

        public CrossAppCompatAutoCompleteTextViewPartialTextTargetBinding(
            CrossAppCompatAutoCompleteTextView target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var autoComplete = View;
            if (autoComplete == null)
            {
                CrossBindingLog.Instance?.LogError(
                    "autoComplete is null in MvxAppCompatAutoCompleteTextViewPartialTextTargetBinding");
            }
        }

        private void AutoCompleteOnPartialTextChanged(object? sender, EventArgs eventArgs)
        {
            FireValueChanged(View?.PartialText);
        }

        public override CrossBindingMode DefaultMode => CrossBindingMode.OneWayToSource;

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            var autoComplete = View;
            if (autoComplete == null)
                return;

            _subscription = autoComplete.WeakSubscribe(
                nameof(autoComplete.PartialTextChanged),
                AutoCompleteOnPartialTextChanged);
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