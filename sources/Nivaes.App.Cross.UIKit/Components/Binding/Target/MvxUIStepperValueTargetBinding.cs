namespace MvvmCross.Platforms.Ios.Binding.Target
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;
    using MvvmCross.Binding.Bindings.Target;
    using Nivaes.App.Cross;

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

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            var stepper = View;
            if (stepper == null)
            {
                MvxBindingLog.Instance?.LogError("UIStepper is null in MvxUIStepperValueTargetBinding");
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