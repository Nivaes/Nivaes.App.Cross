// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Mobile.Droid.Sample
{
    using Android.OS;
    using Android.Runtime;
    using Android.Views;
    using MvvmCross.Platforms.Android.Binding.BindingContext;
    using MvvmCross.Platforms.Android.Views.Fragments;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.Sample;

    [MvxTabLayoutPresentation(TabLayoutResourceId = Resource.Id.tabs, ViewPagerResourceId = Resource.Id.viewpager, Title = "Tab 2", ActivityHostViewModelType = typeof(TabsRootViewModel))]
    [MvxTabLayoutPresentation(TabLayoutResourceId = Resource.Id.tabs, ViewPagerResourceId = Resource.Id.viewpager, Title = "Tab 2", FragmentHostViewType = typeof(TabsRootBView))]
    [Register(nameof(Tab2View))]
    public class Tab2View
        : MvxFragment<Tab2ViewModel>
    {
        public override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Create your fragment here
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.Tab2View, null);

            return view;
        }
    }
}
