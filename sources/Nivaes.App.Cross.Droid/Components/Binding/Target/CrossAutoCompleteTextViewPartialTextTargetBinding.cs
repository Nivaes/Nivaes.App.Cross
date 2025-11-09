namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.Logging;

    public class CrossAutoCompleteTextViewPartialTextTargetBinding
    : MvxAndroidPropertyInfoTargetBinding<CrossAutoCompleteTextView>
    {
        private CrossJavaEventSubscription<CrossAutoCompleteTextView>? _subscription;

        public CrossAutoCompleteTextViewPartialTextTargetBinding(
                object target,
                PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var autoComplete = View;
            if (autoComplete == null)
            {
                CrossBindingLog.Instance?.LogError(
                    "autoComplete is null in {TypeName}", nameof(CrossAutoCompleteTextViewPartialTextTargetBinding));
            }
        }

        private void AutoCompleteOnPartialTextChanged(object? sender, EventArgs eventArgs) =>
            FireValueChanged(View?.PartialText);

        public override CrossBindingMode DefaultMode => CrossBindingMode.TwoWay;

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