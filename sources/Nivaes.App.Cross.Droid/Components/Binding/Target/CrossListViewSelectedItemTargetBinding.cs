namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;

    public class CrossListViewSelectedItemTargetBinding(CrossListView view)
        : CrossAndroidTargetBinding(view)
    {
        private object? _currentValue;
        private CrossAndroidTargetEventSubscription<ListView, AdapterView.ItemClickEventArgs>? _subscription;

        protected CrossListView? ListView => (CrossListView?)Target;

        private void OnItemClick(object? sender, AdapterView.ItemClickEventArgs itemClickEventArgs)
        {
            var listView = ListView;
            if (listView == null)
                return;

            var newValue = listView.Adapter.GetRawItem(itemClickEventArgs.Position);

            if (!newValue.Equals(_currentValue))
            {
                _currentValue = newValue;
                FireValueChanged(newValue);
            }
        }

        protected override void SetValueImpl(object target, object? value)
        {
            if (value == null || value == _currentValue)
                return;

            var listView = (CrossListView)target;

            var index = listView.Adapter.GetPosition(value);
            if (index < 0)
            {
                CrossBindingLog.Instance?.LogWarning("Value not found for spinner {Value}", value);
                return;
            }
            _currentValue = value;
            listView.SetSelection(index);
        }

        public override CrossBindingMode DefaultMode => CrossBindingMode.TwoWay;

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            var listView = (ListView?)ListView;
            if (listView == null)
                return;

            _subscription =
                listView.WeakSubscribe<ListView, AdapterView.ItemClickEventArgs>(nameof(listView.ItemClick), OnItemClick);
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(object);

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