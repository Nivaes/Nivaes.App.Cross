using System.Diagnostics.CodeAnalysis;
using Nivaes.App.Cross.Droid;
using Playground.Droid.Fragments;

namespace Nivaes.App.Cross.Sample.Droid
{
    [MvxFragmentPresentation(typeof(SplitRootViewModel), Resource.Id.split_content_frame, AddToBackStack = true)]
    [RequiresUnreferencedCode("MvxBindings requires unreferenced code")]
    public class SplitDetailNavView : BaseSplitDetailView<SplitDetailNavViewModel>
    {
        protected override int FragmentLayoutId => Resource.Layout.SplitDetailNavView;
    }
}
