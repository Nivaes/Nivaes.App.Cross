using Android.Views;

namespace Nivaes.App.Cross.Droid
{
    public interface IBaseMasterDetailView
    {
        event EventHandler ActivityCreated;
    }

    public abstract class IBaseMasterDetailView<TMasterDetailViewModel>
        : MvxFragment<TMasterDetailViewModel>, IBaseMasterDetailView
        where TMasterDetailViewModel : BaseMasterDetailViewModel
    {
        public IBaseMasterDetailView()
        {
        }

        public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            View view = this.BindingInflate(Resource.Layout.master_detail_view, null);

            return view;
        }

        public override void OnActivityCreated(Bundle savedInstanceState)
        {
            base.OnActivityCreated(savedInstanceState);

            ActivityCreated?.Invoke(this, new EventArgs());
        }

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);

            ViewCreated?.Invoke(this, new EventArgs());
        }

        #region Events
        public event EventHandler ActivityCreated;
        public event EventHandler ViewCreated;
        #endregion
    }
}
