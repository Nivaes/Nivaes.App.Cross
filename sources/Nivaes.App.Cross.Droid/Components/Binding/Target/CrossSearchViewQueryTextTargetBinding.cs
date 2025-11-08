namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;

    public class CrossSearchViewQueryTextTargetBinding
        : CrossAndroidTargetBinding
    {
        private CrossAndroidTargetEventSubscription<SearchView, SearchView.QueryTextChangeEventArgs>? _subscription;

        public CrossSearchViewQueryTextTargetBinding(object target)
            : base(target)
        {
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(string);

        public override CrossBindingMode DefaultMode => CrossBindingMode.TwoWay;

        protected SearchView? SearchView => (SearchView?)Target;

        [RequiresUnreferencedCode("This method may use reflection to subscribe to events which may not be preserved by trimming")]
        public override void SubscribeToEvents()
        {
            _subscription = SearchView?.WeakSubscribe<SearchView, SearchView.QueryTextChangeEventArgs>(
                nameof(SearchView.QueryTextChange),
                HandleQueryTextChanged);
        }

        protected override void SetValueImpl(object target, object? value) =>
            ((SearchView)target).SetQuery((string?)value, true);

        [RequiresUnreferencedCode("This method calls SubscribeToEvents which may use reflection to subscribe to events which may not be preserved by trimming")]
        protected override void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                _subscription?.Dispose();
                _subscription = null;
            }

            base.Dispose(isDisposing);
        }

        private void HandleQueryTextChanged(object? sender, SearchView.QueryTextChangeEventArgs e)
        {
            if (Target is not SearchView target)
                return;

            var value = target.Query;
            FireValueChanged(value);
        }
    }
}