using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.Droid;

namespace Nivaes.App.Cross.Sample.Droid;

[MvxActivityPresentation]
[Activity(Theme = "@style/AppTheme")]
[RequiresUnreferencedCode("Uses MvxBindings which require unreferenced code")]
public sealed class ConvertersActivity
    : MvxActivity<ConvertersViewModel>
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.activity_converters);
    }
}
