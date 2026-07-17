using Android.Views;

namespace Nivaes.App.Cross.Droid
{
    /// <summary>Base default detail view.</summary>
    public abstract class BaseDefaultDetailView<TViewModel>
        : MvxFragment<TViewModel>
        where TViewModel : BaseDefaultDetailViewModel
    {
        protected abstract int LayoutId { get; }

        protected BaseDefaultDetailView()
        {
            base.RetainInstance = false;
        }

        public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = this.BindingInflate(LayoutId, null);
            return view;
        }
    }
}
