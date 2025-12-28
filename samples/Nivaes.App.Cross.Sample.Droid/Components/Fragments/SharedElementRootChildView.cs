// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Android.Views;
using AndroidX.RecyclerView.Widget;
using Nivaes.App.Cross.Droid;
using Playground.Droid.Adapter;

namespace Nivaes.App.Cross.Sample.Droid
{
    [MvxFragmentPresentation(typeof(SharedElementRootViewModel), Resource.Id.shared_content_frame)]
    [RequiresUnreferencedCode("MvxBindings requires unreferenced code")]
    public class SharedElementRootChildView : MvxFragment<SharedElementRootChildViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.SharedElementRootChildView, null);

            var recyclerView = view.FindViewById<MvxRecyclerView>(Resource.Id.my_recycler_view);
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

        private void AdapterOnItemClick(object sender, SelectedItemRecyclerAdapter.SelectedItemEventArgs e)
        {
            Toast.MakeText(Activity, $"Selected item {e.Position + 1}", ToastLength.Short)
                .Show();

            (Activity as SharedElementRootView).SelectedListItem = e.Position;

            ViewModel.SelectItemExecution(e.DataContext as ListItemViewModel);
        }
    }
}
