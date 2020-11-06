//// Licensed to the .NET Foundation under one or more agreements.
//// The .NET Foundation licenses this file to you under the MS-PL license.
//// See the LICENSE file in the project root for more information.


//namespace Nivaes.App.Cross.Mobile.Droid.Sample
//{
//    using System;
//    using Android.OS;
//    using Android.Runtime;
//    using Android.Views;
//    using MvvmCross.Platforms.Android.Binding.BindingContext;
//    using Nivaes.App.Cross.Presenters;
//    using Nivaes.App.Mobile.Sample;

//    [MvxDialogFragmentPresentation]
//    [Register(nameof(SheetView))]
//    public class SheetView
//        : MvxBottomSheetDialogFragment<SheetViewModel>
//    {
//        public SheetView()
//        {
//        }

//        protected SheetView(IntPtr javaReference, JniHandleOwnership transfer)
//            : base(javaReference, transfer)
//        {
//        }

//        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
//        {
//            base.OnCreateView(inflater, container, savedInstanceState);

//            var view = this.BindingInflate(Resource.Layout.SheetView, null);

//            return view;
//        }
//    }
//}
