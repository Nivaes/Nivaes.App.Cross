using Android.Views;
using Nivaes.App.Cross.Droid;

namespace Nivaes.App.Cross.Sample.Droid;

[TabLayoutPresentation(TabLayoutResourceId = Resource.Id.tabs, ViewPagerResourceId = Resource.Id.viewpager, Title = "Tab 1", ActivityHostViewModelType = typeof(TabsRootViewModel))]
[TabLayoutPresentation(TabLayoutResourceId = Resource.Id.tabs, ViewPagerResourceId = Resource.Id.viewpager, Title = "Tab 1", FragmentHostViewType = typeof(TabsRootBView))]
public class Tab1View : MvxFragment<Tab1ViewModel>
{
    public override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
    }

    public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
    {
        base.OnCreateView(inflater, container, savedInstanceState);

        var view = this.BindingInflate(Resource.Layout.Tab1View, container, false);

        return view;
    }
}
