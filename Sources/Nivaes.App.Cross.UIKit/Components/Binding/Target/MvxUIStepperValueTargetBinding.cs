#if IOS || MACCATALYST
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UIKitLib
{
    public class MvxUIStepperValueTargetBinding(
            UIStepper target,
            PropertyInfo targetPropertyInfo)
        : MvxPropertyInfoTargetBinding<UIStepper>(target, targetPropertyInfo)
    {
        private CrossWeakEventSubscription<UIStepper>? _subscription;

        protected override void SetValueImpl(object target, object? value)
        {
            if (target is not UIStepper view || value == null)
                return;

            view.Value = (double)value;
        }

        private void HandleValueChanged(object? sender, EventArgs e)
        {
            var view = View;
            if (view == null) return;

            FireValueChanged(view.Value);
        }

        public override CrossBindingMode DefaultMode => CrossBindingMode.TwoWay;

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            var stepper = View;
            if (stepper == null)
            {
                CrossBindingLogger.Instance?.LogError("UIStepper is null in MvxUIStepperValueTargetBinding");
                return;
            }

            _subscription = stepper.WeakSubscribe(nameof(stepper.ValueChanged), HandleValueChanged);
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
#endif