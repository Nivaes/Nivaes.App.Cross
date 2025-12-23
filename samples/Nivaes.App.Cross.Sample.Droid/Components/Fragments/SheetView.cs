// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.CodeAnalysis;
using Android.Runtime;
using Android.Views;
using Nivaes.App.Cross.Droid;
using Nivaes.App.Cross.Sample.Droid;
using Playground.Core.ViewModels;
using Resource = Nivaes.App.Cross.Sample.Droid.Resource;

namespace Playground.Droid.Fragments
{
    [MvxDialogFragmentPresentation]
    [RequiresUnreferencedCode("MvxBindings require unreferenced code")]
    public class SheetView : MvxBottomSheetDialogFragment<SheetViewModel>
    {
        public SheetView()
        {
        }

        protected SheetView(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);

            var view = this.BindingInflate(Resource.Layout.SheetView, container, false);

            return view;
        }
    }
}
