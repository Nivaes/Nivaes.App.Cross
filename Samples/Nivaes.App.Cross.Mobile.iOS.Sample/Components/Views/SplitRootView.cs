// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Mobile.iOS.Sample
{
    using System;
    using MvvmCross.Platforms.Ios.Views;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.Sample;

    [MvxFromStoryboard("SplitRootView")]
    [MvxRootPresentation]
    public partial class SplitRootView
        : MvxSplitViewController<SplitRootViewModel>
    {
        private bool mIsPresentedFirstTime = true;

        public SplitRootView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            PreferredPrimaryColumnWidthFraction = .3f;
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (ViewModel != null && mIsPresentedFirstTime)
            {
                mIsPresentedFirstTime = false;
                ViewModel.ShowInitialMenuCommand.Execute(null);

                //PreferredDisplayMode = UISplitViewControllerDisplayMode.PrimaryHidden;
            }
        }
    }
}
