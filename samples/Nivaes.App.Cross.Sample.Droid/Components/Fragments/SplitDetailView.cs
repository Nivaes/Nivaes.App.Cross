using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.Droid;

namespace Nivaes.App.Cross.Sample.Droid
{
    [MvxFragmentPresentation(typeof(SplitRootViewModel), Resource.Id.split_content_frame)]
    [RequiresUnreferencedCode("Bindings requires unreferenced code")]
    public class SplitDetailView : BaseSplitDetailView<SplitDetailViewModel>
    {
        protected override int FragmentLayoutId => Resource.Layout.SplitDetailView;
    }
}
