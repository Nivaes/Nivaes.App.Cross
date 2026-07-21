using Android.Views;
using Nivaes.App.Cross.Droid;

namespace Nivaes.App.Cross.Sample.Droid;

[TabLayoutPresentation(TabLayoutResourceId = Resource.Id.tabs, ViewPagerResourceId = Resource.Id.viewpager, Title = "Tab 3")]
public class Tab3View : MvxFragment<Tab3ViewModel>
{
    public override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Create your fragment here
    }

    public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
    {
        base.OnCreateView(inflater, container, savedInstanceState);

        var view = this.BindingInflate(Resource.Layout.Tab3View, container, false);

        return view;
    }
}
