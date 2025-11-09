namespace Nivaes.App.Cross.UIKit
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;

    public class CrossUITextFieldTextTargetBinding(UITextField target)
        : CrossConvertingTargetBinding(target), ICrossEditableTextView
    {
        private CrossWeakEventSubscription<UITextField>? _subscriptionChanged;
        private CrossWeakEventSubscription<UITextField>? _subscriptionEndEditing;

        protected UITextField? View => Target as UITextField;

        private void HandleEditTextValueChanged(object? sender, EventArgs e)
        {
            var view = View;
            if (view == null) return;

            FireValueChanged(view.Text);
        }

        public override CrossBindingMode DefaultMode => CrossBindingMode.TwoWay;

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            var view = View;
            if (view == null)
            {
                CrossBindingLog.Instance?.LogError(
                    "UITextField is null in MvxUITextFieldTextTargetBinding");
                return;
            }

            _subscriptionChanged = view.WeakSubscribe(nameof(target.EditingChanged), HandleEditTextValueChanged);
            _subscriptionEndEditing = view.WeakSubscribe(nameof(target.EditingDidEnd), HandleEditTextValueChanged);
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(string);

        [RequiresUnreferencedCode("This method uses reflection to get type information and perform conversions which may not be preserved by trimming.")]
        protected override bool ShouldSkipSetValueForViewSpecificReasons(object target, object? value)
            => this.ShouldSkipSetValueAsHaveNearlyIdenticalNumericText(target, value);

        protected override void SetValueImpl(object target, object? value)
        {
            var view = (UITextField?)target;
            if (view == null) return;

            view.Text = (string?)value;
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _subscriptionChanged?.Dispose();
                _subscriptionChanged = null;
                _subscriptionEndEditing?.Dispose();
                _subscriptionEndEditing = null;
            }
            base.Dispose(isDisposing);
        }

        public string? CurrentText
        {
            get
            {
                var view = View;
                return view?.Text;
            }
        }
    }
}