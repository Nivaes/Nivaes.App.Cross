namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;
    using MvvmCross.Platforms.Android.Binding.Views;

    public class MvxAutoCompleteTextViewPartialTextTargetBinding
        : MvxAndroidPropertyInfoTargetBinding<MvxAutoCompleteTextView>
    {
        private CrossJavaEventSubscription<MvxAutoCompleteTextView>? _subscription;

        public MvxAutoCompleteTextViewPartialTextTargetBinding(
                object target,
                PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var autoComplete = View;
            if (autoComplete == null)
            {
                CrossBindingLogger.Instance?.LogError(
                    "autoComplete is null in {TypeName}", nameof(MvxAutoCompleteTextViewPartialTextTargetBinding));
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