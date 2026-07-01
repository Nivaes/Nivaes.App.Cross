namespace MvvmCross.Platforms.Ios.Binding.Target
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    public class MvxUITextViewTextTargetBinding(UITextView target)
    : CrossConvertingTargetBinding(target)
    {
        private CrossWeakEventSubscription<NSTextStorage, NSTextStorageEventArgs>? _subscription;

        protected UITextView? View => Target as UITextView;

        private void EditTextOnChanged(object? sender, NSTextStorageEventArgs eventArgs)
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
                CrossBindingLogger.Instance?.LogError(
                    "UITextView is null in MvxUITextViewTextTargetBinding");
                return;
            }

            var textStorage = view.LayoutManager.TextStorage;
            if (textStorage == null)
            {
                CrossBindingLogger.Instance?.LogError(
                    "NSTextStorage of UITextView is null in MvxUITextViewTextTargetBinding");
                return;
            }

            _subscription =
                textStorage.WeakSubscribe<NSTextStorage, NSTextStorageEventArgs>(nameof(textStorage.DidProcessEditing),
                    EditTextOnChanged);
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(string);

        protected override void SetValueImpl(object target, object? value)
        {
            var view = (UITextView?)target;
            if (view == null) return;

            view.Text = (string?)value;
        }

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing) return;

            _subscription?.Dispose();
            _subscription = null;
        }
    }
}