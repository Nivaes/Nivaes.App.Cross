#if IOS || MACCATALYST
namespace Nivaes.App.Cross.UIKitOS
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;
    using Nivaes.App.Cross;

    public class MvxUISwitchOnTargetBinding(UISwitch target)
        : MvxTargetBinding<UISwitch, bool>(target)
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
#endif