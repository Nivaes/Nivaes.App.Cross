namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Binding;
    using MvvmCross.Platforms.Android.Binding.Views;

    public class MvxListViewSelectedItemTargetBinding(MvxListView view)
    : MvxAndroidTargetBinding(view)
    {
        private object? _currentValue;
        private CrossAndroidTargetEventSubscription<ListView, AdapterView.ItemClickEventArgs>? _subscription;

        protected MvxListView? ListView => (MvxListView?)Target;

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

            var listView = (MvxListView)target;

            var index = listView.Adapter.GetPosition(value);
            if (index < 0)
            {
                CrossBindingLogger.Instance?.LogWarning("Value not found for spinner {Value}", value);
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

            _subscription = listView.DroidWeakSubscribe<ListView, AdapterView.ItemClickEventArgs>(nameof(listView.ItemClick), OnItemClick);
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