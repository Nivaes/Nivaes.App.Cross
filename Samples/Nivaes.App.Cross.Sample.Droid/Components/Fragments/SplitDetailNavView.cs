using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.Droid;

namespace Nivaes.App.Cross.Sample.Droid
{
    [FragmentPresentation(typeof(SplitRootViewModel), Resource.Id.split_content_frame, AddToBackStack = true)]
    [RequiresUnreferencedCode("Bindings requires unreferenced code")]
    public class SplitDetailNavView : BaseSplitDetailView<SplitDetailNavViewModel>
    {
        protected override int FragmentLayoutId => Resource.Layout.SplitDetailNavView;
    }
}
