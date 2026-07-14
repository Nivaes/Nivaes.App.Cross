using System.Diagnostics.CodeAnalysis;
using Android.Views;
using Nivaes.App.Cross.Droid;

namespace Nivaes.App.Cross.Sample.Droid;

[RequiresUnreferencedCode("Bindings requires unreferenced code")]
[FragmentPresentation(typeof(RootViewModel), Resource.Id.content_frame, true,
                         Resource.Animation.abc_fade_in,
                         Resource.Animation.abc_fade_out,
                         Resource.Animation.abc_fade_in,
                         Resource.Animation.abc_fade_out)]
[FragmentPresentation(typeof(SplitRootViewModel), Resource.Id.split_content_frame)]
[FragmentPresentation(typeof(TabsRootViewModel), Resource.Id.content_frame)]
[FragmentPresentation(fragmentHostViewType: typeof(ModalNavView), fragmentContentId: Resource.Id.dialog_content_frame)]
public class ChildView : MvxFragment<ChildViewModel>
{
    public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
    {
        base.OnCreateView(inflater, container, savedInstanceState);

        var view = this.BindingInflate(Resource.Layout.ChildView, container, false);

        return view!;
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}
