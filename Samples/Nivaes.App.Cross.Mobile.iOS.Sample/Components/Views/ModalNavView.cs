// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Mobile.iOS.Sample
{
    using System;
    using MvvmCross.Platforms.Ios.Presenters.Attributes;
    using MvvmCross.Platforms.Ios.Views;
    using Nivaes.App.Cross.Sample;
    using UIKit;

    [MvxFromStoryboard("ModalNavView")]
    [MvxModalPresentation(WrapInNavigationController = true)]
    public partial class ModalNavView : MvxViewController<ModalNavViewModel>
    {
        public ModalNavView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.Red;

            var set = CreateBindingSet();
            set.Bind(btnShowChild).To(vm => vm.ShowChildCommand);
            set.Bind(btnClose).To(vm => vm.CloseCommand);
            set.Bind(btnNestedModal).To(vm => vm.ShowNestedModalCommand);
            set.Apply();
        }
    }
}
