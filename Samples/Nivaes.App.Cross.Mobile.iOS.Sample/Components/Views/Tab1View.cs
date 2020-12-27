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
    [MvxTabPresentation(WrapInNavigationController = true, TabIconName = "home", TabName = "Tab 1")]
    public partial class Tab1View
        : MvxViewController<Tab1ViewModel>
    {
        public Tab1View(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            var set = CreateBindingSet();
            set.Bind(btnModal).To(vm => vm.OpenModalCommand);
            set.Bind(btnNavModal).To(vm => vm.OpenNavModalCommand);
            set.Bind(btnChild).To(vm => vm.OpenChildCommand);
            set.Bind(btnTab2).To(vm => vm.OpenTab2Command);
            set.Apply();
        }
    }
}
