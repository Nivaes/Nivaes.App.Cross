using System.Diagnostics.CodeAnalysis;
using Android.Content;
using Android.Content.Res;
using Android.Views;
using MvvmCross.Platforms.Android.Views.AppCompat;
using Nivaes.App.Cross;
using Nivaes.App.Cross.Droid;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace Nivaes.App.Cross.Sample.Droid;

[RequiresUnreferencedCode("Uses MvxBindings which require unreferenced code")]
public abstract class BaseSplitDetailView<TViewModel>
    : MvxFragment<TViewModel> where TViewModel : class, ICrossViewModel
{
    protected SplitRootView? BaseActivity => (SplitRootView?)Activity;
    protected Toolbar? _toolbar;
    protected MvxActionBarDrawerToggle? _drawerToggle;

    protected abstract int FragmentLayoutId { get; }

    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        base.OnCreateView(inflater, container, savedInstanceState);

        var view = this.BindingInflate(FragmentLayoutId, container, false);

        _toolbar = view?.FindViewById<Toolbar>(Resource.Id.toolbar);
        if (_toolbar != null)
        {
            BaseActivity?.SetSupportActionBar(_toolbar);
            BaseActivity?.SupportActionBar?.SetDisplayHomeAsUpEnabled(true);

            _drawerToggle = new MvxActionBarDrawerToggle(
                Activity,                               // host Activity
                BaseActivity.DrawerLayout,              // DrawerLayout object
                _toolbar,                               // nav drawer icon to replace 'Up' caret
                Resource.String.drawer_open,            // "open drawer" description
                Resource.String.drawer_close            // "close drawer" description
            );
            BaseActivity.DrawerLayout.AddDrawerListener(_drawerToggle);
            _drawerToggle.DrawerIndicatorEnabled = true;
        }

        return view;
    }

    public override void OnConfigurationChanged(Configuration newConfig)
    {
        base.OnConfigurationChanged(newConfig);
        if (_toolbar != null)
            _drawerToggle?.OnConfigurationChanged(newConfig);
    }

    public override void OnAttach(Context context)
    {
        base.OnAttach(context);
        if (_toolbar != null)
            _drawerToggle?.SyncState();
    }
}
