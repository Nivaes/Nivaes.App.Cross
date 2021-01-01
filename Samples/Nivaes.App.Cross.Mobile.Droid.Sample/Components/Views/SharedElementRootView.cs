// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Mobile.Droid.Sample
{
    using System.Collections.Generic;
    using Android.App;
    using Android.OS;
    using Android.Views;
    using Android.Widget;
    using MvvmCross.Platforms.Android.Views;
    using Nivaes.App.Cross.ViewModels;
    using Nivaes.App.Cross.Droid.RecyclerView;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.Sample;

    [MvxActivityPresentation]
    [Activity(Theme = "@style/AppTheme")]
    public class SharedElementRootView
        : MvxActivity<SharedElementRootViewModel>, IMvxAndroidSharedElements
    {
        public int SelectedListItem { get; set; }

        public IDictionary<string, View> FetchSharedElementsToAnimate(MvxBasePresentationAttribute attribute, MvxViewModelRequest request)
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

        protected override void OnCreate(Bundle? bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.SharedElementRootView);
        }
    }
}
