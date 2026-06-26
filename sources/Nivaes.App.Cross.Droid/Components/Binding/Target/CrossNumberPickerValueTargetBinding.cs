namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    public class CrossNumberPickerValueTargetBinding(
            object target,
            PropertyInfo targetPropertyInfo)
        : MvxPropertyInfoTargetBinding<NumberPicker>(target, targetPropertyInfo)
    {
        private CrossAndroidTargetEventSubscription<NumberPicker, NumberPicker.ValueChangeEventArgs>? _subscription;


        public override CrossBindingMode DefaultMode => CrossBindingMode.TwoWay;

        protected override void SetValueImpl(object target, object? value)
        {
            var numberPicker = (NumberPicker?)target;
            if (numberPicker == null)
                return;

            if (value != null)
                numberPicker.Value = (int)value;
        }

        private void NumberPickerValueChanged(object? sender, NumberPicker.ValueChangeEventArgs e)
        {
            if (!e.OldVal.Equals(e.NewVal))
                FireValueChanged(e.NewVal);
        }

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            var numberPicker = View;
            if (numberPicker == null)
            {
                CrossBindingLogger.Instance?.LogError("NumberPicker is null in MvxNumberPickerValueTargetBinding");
                return;
            }

            _subscription = numberPicker.DroidWeakSubscribe<NumberPicker, NumberPicker.ValueChangeEventArgs>(
                nameof(numberPicker.ValueChanged),
                NumberPickerValueChanged);
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