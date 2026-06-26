namespace MvvmCross.Platforms.Android.Binding.Target
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Droid;

    public class MvxAppCompatAutoCompleteTextViewPartialTextTargetBinding
        : MvxAndroidPropertyInfoTargetBinding<MvxAppCompatAutoCompleteTextView>
    {
        private CrossJavaEventSubscription<MvxAppCompatAutoCompleteTextView>? _subscription;

        public MvxAppCompatAutoCompleteTextViewPartialTextTargetBinding(
            MvxAppCompatAutoCompleteTextView target, PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var autoComplete = View;
            if (autoComplete == null)
            {
                CrossBindingLogger.Instance?.LogError(
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

            _subscription = autoComplete.DroidWeakSubscribe(
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