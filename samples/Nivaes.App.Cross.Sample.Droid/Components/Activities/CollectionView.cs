using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.Droid;
using Playground.Core.ViewModels;

namespace Nivaes.App.Cross.Sample.Droid;

[MvxActivityPresentation]
[Activity(Theme = "@style/AppTheme")]
[RequiresUnreferencedCode("Uses Bindings which require unreferenced code")]
public sealed class CollectionView 
    : MvxActivity<CollectionViewModel>
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.CollectionView);
    }
}
