using System.Diagnostics.CodeAnalysis;
using Android.Views;
using AndroidX.Core.View;
using Nivaes.App.Cross.Droid;

namespace Nivaes.App.Cross.Sample.Droid;

[MvxActivityPresentation]
[Activity(Theme = "@style/AppTheme",
    WindowSoftInputMode = SoftInput.AdjustPan)]
[RequiresUnreferencedCode("Uses MvxBindings which require unreferenced code")]
public sealed class RootView : MvxActivity<RootViewModel>, IOnApplyWindowInsetsListener
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.RootView);

        ViewCompat.SetOnApplyWindowInsetsListener(FindViewById(Resource.Id.main_frame), this);
    }

    public WindowInsetsCompat OnApplyWindowInsets(View? v, WindowInsetsCompat? insets)
    {
        var inset = insets?.GetInsets(WindowInsetsCompat.Type.SystemBars());

        (v.LayoutParameters as FrameLayout.LayoutParams).SetMargins(
            inset.Left,
            inset.Top,
            inset.Right,
            inset.Bottom);

        return WindowInsetsCompat.Consumed;
    }
}
