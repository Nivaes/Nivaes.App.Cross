using System.Diagnostics.CodeAnalysis;
using Android.Content.PM;
using AndroidX.Core.View;
using AndroidX.DrawerLayout.Widget;
using Nivaes.App.Cross.Droid;
using Playground.Droid.Extensions;

namespace Nivaes.App.Cross.Sample.Droid;

[MvxActivityPresentation]
[Activity(
    Theme = "@style/AppTheme",
    LaunchMode = LaunchMode.SingleTop,
    ConfigurationChanges = ConfigChanges.Orientation | ConfigChanges.ScreenSize)]
[RequiresUnreferencedCode("Uses MvxBindings which require unreferenced code")]
public class SplitRootView : MvxActivity<SplitRootViewModel>
{
    public DrawerLayout DrawerLayout { get; set; }

    protected override void OnCreate(Android.OS.Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.SplitRootView);

        DrawerLayout = FindViewById<DrawerLayout>(Resource.Id.drawer_layout);

        if (savedInstanceState == null)
        {
            ViewModel.ShowInitialMenuCommand.Execute();
            ViewModel.ShowDetailCommand.Execute();
        }

        OnBackPressedDispatcher.AddCallback(this, new BackPressedCallback(true, BackPressed));
    }

    private void BackPressed()
    {
        if (DrawerLayout?.IsDrawerOpen(GravityCompat.Start) is true)
            DrawerLayout.CloseDrawers();
        else
            Finish();
    }
}
