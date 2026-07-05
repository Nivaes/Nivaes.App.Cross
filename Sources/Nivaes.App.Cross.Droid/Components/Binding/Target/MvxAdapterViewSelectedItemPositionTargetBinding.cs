using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross.Droid
{
    public class MvxAdapterViewSelectedItemPositionTargetBinding(AdapterView adapterView)
        : MvxAndroidTargetBinding(adapterView)
    {
        private CrossAndroidTargetEventSubscription<AdapterView, AdapterView.ItemSelectedEventArgs>? _subscription;

        private AdapterView? AdapterView => (AdapterView?)Target;

        protected override void SetValueImpl(object target, object? value)
        {
            if (value != null)
                AdapterView?.SetSelection((int)value);
        }

        private void AdapterViewOnItemSelected(object? sender, AdapterView.ItemSelectedEventArgs itemSelectedEventArgs)
        {
            FireValueChanged(itemSelectedEventArgs.Position);
        }

        public override CrossBindingMode DefaultMode => CrossBindingMode.TwoWay;

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            var view = AdapterView;

            if (view == null)
                return;

            _subscription = view.DroidWeakSubscribe<AdapterView, AdapterView.ItemSelectedEventArgs>(
                nameof(adapterView.ItemSelected), AdapterViewOnItemSelected);
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(int);

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