using System.Diagnostics.CodeAnalysis;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Nivaes.App.Cross.Droid;
using Nivaes.App.Cross.Droid.RecyclerView;
using Playground.Droid.Adapter;

namespace Nivaes.App.Cross.Sample.Droid
{
    [MvxFragmentPresentation(typeof(SharedElementRootViewModel), Resource.Id.shared_content_frame)]
    [RequiresUnreferencedCode("Bindings requires unreferenced code")]
    public class SharedElementRootChildView : MvxFragment<SharedElementRootChildViewModel>
    {
        public SharedElementRootChildView()
        {
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.SharedElementRootChildView, null);

            var recyclerView = view!.FindViewById<MvxRecyclerView>(Resource.Id.my_recycler_view);
            if (recyclerView != null)
            {
                recyclerView.HasFixedSize = true;
                var layoutManager = new LinearLayoutManager(Activity);
                recyclerView.SetLayoutManager(layoutManager);

                var adapter = new SelectedItemRecyclerAdapter(BindingContext as IMvxAndroidBindingContext);
                adapter.OnItemClick += AdapterOnItemClick;
                recyclerView.Adapter = adapter;
            }

            return view;
        }

        private void AdapterOnItemClick(object? sender, SelectedItemRecyclerAdapter.SelectedItemEventArgs e)
        {
            Toast.MakeText(Activity, $"Selected item {e.Position + 1}", ToastLength.Short)?.Show();

            ((SharedElementRootView?)Activity)?.SelectedListItem = e.Position;

            ViewModel.SelectItemExecution((ListItemViewModel)e.DataContext!);
        }
    }
}
