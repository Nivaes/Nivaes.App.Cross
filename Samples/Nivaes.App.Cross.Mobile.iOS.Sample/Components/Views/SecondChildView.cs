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

    [MvxFromStoryboard("SecondChildView")]
    public partial class SecondChildView : MvxViewController<SecondChildViewModel>
    {
        public SecondChildView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.Green;

            var set = CreateBindingSet();
            set.Bind(btnClose).To(vm => vm.CloseCommand);
            set.Apply();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            btnCloseStack.TouchUpInside += BtnCloseStack_TouchUpInside;
        }

        public override void ViewDidDisappear(bool animated)
        {
            base.ViewDidDisappear(animated);

            btnCloseStack.TouchUpInside -= BtnCloseStack_TouchUpInside;
        }

        private void BtnCloseStack_TouchUpInside(object sender, EventArgs e)
        {
            var appDelegate = UIApplication.SharedApplication.Delegate as AppDelegate;
            var presenter = Mvx.IoCProvider.GetSingleton<IMvxIosViewPresenter>() as MvxIosViewPresenter;

            if (appDelegate.Window.RootViewController.PresentedViewController != null)
            {
                appDelegate.Window.RootViewController.DismissViewController(true, null);
                _ = presenter.CloseModalViewControllers().AsTask();
            }
            else
            {
                presenter.MasterNavigationController.PopToRootViewController(true);
            }
        }
    }
}
