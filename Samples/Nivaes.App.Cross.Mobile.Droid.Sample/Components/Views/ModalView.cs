// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Mobile.Droid.Sample
{
    using System;
    using Android.OS;
    using Android.Runtime;
    using Android.Views;
    using MvvmCross.Platforms.Android;
    using MvvmCross.Platforms.Android.Binding.BindingContext;
    using MvvmCross.Platforms.Android.Views.Fragments;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.Sample;

    [MvxDialogFragmentPresentation]
    [Register(nameof(ModalView))]
    public class ModalView
        : MvxDialogFragment<ModalViewModel>
    {
        public ModalView()
        {
        }

        protected ModalView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            var ignore = base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.ChildView, null);

            return view;
        }

        public override void OnPause()
        {
            var top = Mvx.IoCProvider.Resolve<IMvxAndroidCurrentTopActivity>();
            var activity = top.Activity;

            base.OnPause();
        }
    }
}
