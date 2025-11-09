namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.Logging;

    public class CrossAutoCompleteTextViewSelectedObjectTargetBinding
        : MvxAndroidPropertyInfoTargetBinding<CrossAutoCompleteTextView>
    {
        private CrossJavaEventSubscription<CrossAutoCompleteTextView>? _subscription;

        public CrossAutoCompleteTextViewSelectedObjectTargetBinding(
                CrossAutoCompleteTextView target,
                PropertyInfo targetPropertyInfo)
            : base(target, targetPropertyInfo)
        {
            var autoComplete = View;
            if (autoComplete == null)
            {
                CrossBindingLog.Instance?.LogError(
                    "autoComplete is null in {TypeName}", nameof(CrossAutoCompleteTextViewSelectedObjectTargetBinding));
            }
        }

        private void AutoCompleteOnSelectedObjectChanged(object? sender, EventArgs eventArgs)
        {
            FireValueChanged(View?.SelectedObject);
        }

        public override CrossBindingMode DefaultMode => CrossBindingMode.TwoWay;

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            var autoComplete = View;
            if (autoComplete == null)
                return;

            _subscription = autoComplete.WeakSubscribe(
                nameof(autoComplete.SelectedObjectChanged),
                AutoCompleteOnSelectedObjectChanged);
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
    }
}