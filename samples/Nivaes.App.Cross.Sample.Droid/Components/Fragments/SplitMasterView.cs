using System.Diagnostics.CodeAnalysis;
using Android.Views;
using Google.Android.Material.Navigation;
using Nivaes.App.Cross.Droid;

namespace Nivaes.App.Cross.Sample.Droid;

[MvxFragmentPresentation(typeof(SplitRootViewModel), Resource.Id.split_navigation_frame)]
[RequiresUnreferencedCode("Bindings requires unreferenced code")]
public class SplitMasterView 
    : MvxFragment<SplitMasterViewModel>, NavigationView.IOnNavigationItemSelectedListener
{
    private IMenuItem? previousMenuItem;

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        var ignore = base.OnCreateView(inflater, container, savedInstanceState);

        var view = this.BindingInflate(Resource.Layout.SplitMasterView, container, false);

        return view;
    }

    public bool OnNavigationItemSelected(IMenuItem menuItem)
    {
        menuItem.SetCheckable(true);
        menuItem.SetChecked(true);
        previousMenuItem?.SetChecked(false);
        previousMenuItem = menuItem;

        Navigate(menuItem.ItemId);

        return true;
    }

    private Task Navigate(int itemId)
    {
        ((SplitRootView)Activity).DrawerLayout.CloseDrawers();
        return Task.Delay(TimeSpan.FromMilliseconds(250));

        //switch (itemId)
        //{
        //    case Resource.Id.nav_home:
        //        break;
        //}
    }
}
