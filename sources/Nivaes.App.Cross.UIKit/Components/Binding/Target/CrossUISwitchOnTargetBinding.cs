namespace Nivaes.App.Cross.UIKit
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;

    public class CrossUISwitchOnTargetBinding(UISwitch target)
        : CrossTargetBinding<UISwitch, bool>(target)
    {
        private CrossWeakEventSubscription<UISwitch>? _subscription;

        [RequiresUnreferencedCode("This method may perform type conversions which may not be preserved by trimming")]
        protected override void SetValue(bool value)
        {
            Target?.SetState(value, true);
        }

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            var uiSwitch = Target;
            if (uiSwitch == null)
            {
                CrossBindingLog.Instance?.LogError("Switch is null in MvxUISwitchOnTargetBinding");
                return;
            }

            _subscription = uiSwitch.WeakSubscribe(nameof(uiSwitch.ValueChanged), HandleValueChanged);
        }

        public override CrossBindingMode DefaultMode => CrossBindingMode.TwoWay;

        [RequiresUnreferencedCode("Binding functionality accesses members dynamically through reflection.")]
        protected override void Dispose(bool isDisposing)
        {
            base.Dispose(isDisposing);
            if (!isDisposing) return;

            _subscription?.Dispose();
            _subscription = null;
        }

        private void HandleValueChanged(object? sender, EventArgs e)
        {
            FireValueChanged(Target?.On ?? false);
        }
    }
}