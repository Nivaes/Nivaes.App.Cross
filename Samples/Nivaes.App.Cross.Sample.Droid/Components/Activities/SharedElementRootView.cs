using System.Diagnostics.CodeAnalysis;
using Android.Views;
using Nivaes.App.Cross.Droid;
using Nivaes.App.Cross.Droid.RecyclerView;

namespace Nivaes.App.Cross.Sample.Droid;

[ActivityPresentation]
[Activity(Theme = "@style/AppTheme")]
[RequiresUnreferencedCode("Uses Bindings which require unreferenced code")]
public sealed class SharedElementRootView
    : CrossActivity<SharedElementRootViewModel>, IMvxAndroidSharedElements
{
    public int SelectedListItem { get; set; }

    public IDictionary<string, View> FetchSharedElementsToAnimate(BasePresentationAttribute attribute, IViewModelRequest request)
    {
        IDictionary<string, View> sharedElements = new Dictionary<string, View>();

        var recyclerView = FindViewById<MvxRecyclerView>(Resource.Id.my_recycler_view);
        if (recyclerView != null)
        {
            var selectedViewHolder = recyclerView.FindViewHolderForAdapterPosition(SelectedListItem);

            var selectedMvxLogo = selectedViewHolder.ItemView.FindViewById<ImageView>(Resource.Id.img_logo);
            sharedElements.Add(nameof(Resource.Id.img_logo), selectedMvxLogo);
        }

        return sharedElements;
    }

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        SetContentView(Resource.Layout.SharedElementRootView);
    }
}
