using System.Diagnostics.CodeAnalysis;
using AndroidX.ViewPager.Widget;
using Nivaes.App.Cross.Droid;

namespace Nivaes.App.Cross.Sample.Droid;

[MvxActivityPresentation]
[Activity(Theme = "@style/AppTheme", ConfigurationChanges = Android.Content.PM.ConfigChanges.Orientation | Android.Content.PM.ConfigChanges.ScreenSize)]
[RequiresUnreferencedCode("Bindings require unreferenced code")]
public class TabsRootView : CrossActivity<TabsRootViewModel>
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.TabsRootView);

        var viewPager = FindViewById<ViewPager>(Resource.Id.viewpager);
        if (viewPager!.Adapter is not MvxCachingFragmentStatePagerAdapter)
            viewPager.Adapter = new MvxCachingFragmentStatePagerAdapter(SupportFragmentManager, new());

        if (savedInstanceState == null)
        {
            ViewModel!.ShowInitialViewModelsCommand.Execute();
        }
    }
}
