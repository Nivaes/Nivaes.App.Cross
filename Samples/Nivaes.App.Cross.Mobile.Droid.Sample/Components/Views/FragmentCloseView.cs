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

    [Register(nameof(FragmentCloseView))]
    //[MvxFragmentPresentation(typeof(RootViewModel), Resource.Id.content_frame, true)]
    [MvxFragmentPresentation(typeof(RootViewModel), Resource.Id.content_frame, true, popBackStackImmediateName: null, popBackStackImmediateFlag: MvxPopBackStack.None)]
    public class FragmentCloseView
        : MvxFragment<FragmentCloseViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            return this.BindingInflate(Resource.Layout.FragmnetCloseView, null);
        }
    }
}
