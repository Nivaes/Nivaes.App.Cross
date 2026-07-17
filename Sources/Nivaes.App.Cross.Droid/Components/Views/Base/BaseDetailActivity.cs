namespace Nivaes.App.Cross.Droid
{
    public abstract class BaseDetailActivity<TViewModel>
        : BaseActivity<TViewModel>, IMainActivity
        where TViewModel : BaseDetailViewModel
    {
        protected override int LayoutId { get; } = Resource.Layout.main_detail_activity;
    }
}
