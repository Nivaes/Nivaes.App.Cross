// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Mobile.iOS.Sample
{
    using System;
    using MvvmCross.Platforms.Ios.Views;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.Sample;

    [MvxFromStoryboard("SplitMasterView")]
    [MvxSplitViewPresentation(MasterDetailPosition.Master)]
    public partial class SplitMasterView
        : MvxViewController<SplitMasterViewModel>
    {
        public SplitMasterView(IntPtr handle)
            : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = CreateBindingSet();
            set.Bind(btnDetail).To(vm => vm.OpenDetailCommand);
            set.Bind(btnDetailNav).To(vm => vm.OpenDetailNavCommand);
            set.Bind(btnStack).To(vm => vm.ShowRootViewModel);
            set.Apply();
        }
    }
}
