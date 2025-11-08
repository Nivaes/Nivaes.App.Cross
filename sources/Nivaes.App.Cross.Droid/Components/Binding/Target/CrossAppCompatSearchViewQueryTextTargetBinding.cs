namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using SearchView = AndroidX.AppCompat.Widget.SearchView;

    public class CrossAppCompatSearchViewQueryTextTargetBinding
        : CrossAndroidTargetBinding
    {
        private IDisposable? _subscription;

        public CrossAppCompatSearchViewQueryTextTargetBinding(SearchView target)
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

        protected override void SetValueImpl(object target, object? value)
        {
            if (target is SearchView searchView)
                searchView.SetQuery((string?)value, true);
        }

        private void HandleQueryTextChanged(object? sender, SearchView.QueryTextChangeEventArgs e)
        {
            if (Target is not SearchView searchView)
                return;

            var value = searchView.Query;
            FireValueChanged(value);
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