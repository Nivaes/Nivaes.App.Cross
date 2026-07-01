using System.Diagnostics.CodeAnalysis;
using Android.Views;
using AndroidX.ViewPager.Widget;
using Nivaes.App.Cross.Droid;

namespace Nivaes.App.Cross.Sample.Droid;

[MvxFragmentPresentation(fragmentHostViewType: typeof(SplitDetailView), fragmentContentId: Resource.Id.tabs_frame, addToBackStack: true)]
[RequiresUnreferencedCode("Bindings requires unreferenced code")]
public class TabsRootBView : MvxFragment<TabsRootBViewModel>
{
    public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
    {
        base.OnCreateView(inflater, container, savedInstanceState);

        var view = this.BindingInflate(Resource.Layout.TabsRootBView, container, false);

        return view;
    }

    public override void OnViewCreated(View view, Bundle? savedInstanceState)
    {
        base.OnViewCreated(view, savedInstanceState);

        var viewPager = view.FindViewById<ViewPager>(Resource.Id.viewpager);
        if (viewPager?.Adapter is not MvxCachingFragmentStatePagerAdapter)
            viewPager?.Adapter = new MvxCachingFragmentStatePagerAdapter(ChildFragmentManager, []);

        if (savedInstanceState == null)
        {
            ViewModel.ShowInitialViewModelsCommand.Execute();
        }
    }
}
