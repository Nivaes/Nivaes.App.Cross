using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.Droid;

namespace Nivaes.App.Cross.Sample.Droid;

[MvxActivityPresentation]
[Activity(Label = "View for CustomBindingViewModel", Theme = "@style/AppTheme")]
[RequiresUnreferencedCode("Uses Bindings which require unreferenced code")]
public sealed class CustomBindingView : CrossActivity<CustomBindingViewModel>
{
    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(Resource.Layout.CustomBindingView);
    }
}
