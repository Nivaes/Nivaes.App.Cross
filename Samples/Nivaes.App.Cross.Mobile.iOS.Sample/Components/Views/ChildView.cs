// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Mobile.iOS.Sample
{
    using System;
    using MvvmCross.Platforms.Ios.Views;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.Sample;
    using UIKit;

    [MvxFromStoryboard("ChildView")]
    [MvxChildPresentation]
    public partial class ChildView : MvxViewController<ChildViewModel>
    {
        public ChildView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.Yellow;

            var set = CreateBindingSet();
            set.Bind(btnClose).To(vm => vm.CloseCommand);
            set.Bind(btnShowSecondChild).To(vm => vm.ShowSecondChildCommand);
            set.Apply();
        }
    }
}
