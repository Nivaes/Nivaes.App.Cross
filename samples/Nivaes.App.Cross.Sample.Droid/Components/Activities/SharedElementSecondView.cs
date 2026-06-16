using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.Droid;

namespace Nivaes.App.Cross.Sample.Droid;

[MvxActivityPresentation]
[Activity(Theme = "@style/AppTheme")]
[RequiresUnreferencedCode("Uses Bindings which require unreferenced code")]
public sealed class SharedElementSecondView 
    : MvxActivity<SharedElementSecondViewModel>
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.SharedElementSecondView);

        Bundle extras = Intent.Extras;
        extras.SetSharedElementsById(FindViewById(Android.Resource.Id.Content));
    }
}
