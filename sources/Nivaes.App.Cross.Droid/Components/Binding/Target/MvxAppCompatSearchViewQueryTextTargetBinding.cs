namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Binding;
    using MvvmCross.Platforms.Android.Binding.Target;
    using SearchView = AndroidX.AppCompat.Widget.SearchView;

    public class MvxAppCompatSearchViewQueryTextTargetBinding
    : MvxAndroidTargetBinding
    {
        private IDisposable? _subscription;

        public MvxAppCompatSearchViewQueryTextTargetBinding(SearchView target)
            : base(target)
        {
        }

        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public override Type TargetValueType => typeof(string);

        public override MvxBindingMode DefaultMode => MvxBindingMode.TwoWay;

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