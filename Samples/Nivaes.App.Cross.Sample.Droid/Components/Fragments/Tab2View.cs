using Android.Views;
using Nivaes.App.Cross.Droid;

namespace Nivaes.App.Cross.Sample.Droid
{
    [TabLayoutPresentation(TabLayoutResourceId = Resource.Id.tabs, ViewPagerResourceId = Resource.Id.viewpager, Title = "Tab 2", ActivityHostViewModelType = typeof(TabsRootViewModel))]
    [TabLayoutPresentation(TabLayoutResourceId = Resource.Id.tabs, ViewPagerResourceId = Resource.Id.viewpager, Title = "Tab 2", FragmentHostViewType = typeof(TabsRootBView))]
    public class Tab2View : MvxFragment<Tab2ViewModel>
    {
        public override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
        }

        public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.Tab2View, container, false);

            return view;
        }
    }
}
