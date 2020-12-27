// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Mobile.iOS.Sample
{
    using System;
    using MvvmCross.Platforms.Ios.Views;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.Sample;

    [MvxFromStoryboard("TabsRootView")]
    [MvxTabPresentation(WrapInNavigationController = false)]
    public partial class Tab3View
        : MvxViewController<Tab3ViewModel>, IMvxTabBarItemViewController
    {
        public Tab3View(IntPtr handle) : base(handle)
        {
        }

        public string TabName => "Third";
        public string TabIconName => "settings";

        public string TabSelectedIconName => "settings";

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = CreateBindingSet();
            set.Bind(btnShowStack).To(vm => vm.ShowRootViewModelCommand);
            set.Bind(btnClose).To(vm => vm.CloseViewModelCommand);
            set.Apply();
        }
    }
}
