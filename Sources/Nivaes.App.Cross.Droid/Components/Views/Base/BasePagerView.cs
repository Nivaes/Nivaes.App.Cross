using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace Nivaes.App.Cross.Droid
{
    public abstract class BasePagerView<TViewModel>
        : MvxFragment<TViewModel>
        where TViewModel : IBasePagerViewModel, ICrossViewModel
    {
        protected abstract int LayoutId { get; }

        protected string Title { get; set; }

        protected Toolbar DetailToolbar { get; private set; }

        protected BasePagerView()
        {
        }

        //public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        //{
        //    base.OnCreateView(inflater, container, savedInstanceState);
        //    var view = this.BindingInflate(LayoutId, null);

        //    if (base.Activity is IMainActivity)
        //    {
        //        DetailToolbar = view.FindViewById<Toolbar>(Resource.Id.detail_toolbar);

        //        if (DetailToolbar != null)
        //        {
        //            if (!string.IsNullOrEmpty(Title))
        //            {
        //                DetailToolbar.Title = Title;
        //            }
        //        }
        //    }

        //    return view;
        //}
    }
}
