// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Mobile.iOS.Sample
{
    using System;
    using MvvmCross.Platforms.Ios.Presenters.Attributes;
    using MvvmCross.Platforms.Ios.Views;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.Sample;
    using Nivaes.App.Cross.ViewModels;
    using UIKit;

    [MvxFromStoryboard("Main")]
    public partial class OverrideAttributeView
        : MvxViewController<OverrideAttributeViewModel>, IMvxOverridePresentationAttribute
    {
        public OverrideAttributeView(IntPtr handle) : base(handle)
        {
        }

        public MvxBasePresentationAttribute PresentationAttribute(MvxViewModelRequest request)
        {
            return new MvxModalPresentationAttribute
            {
                ModalPresentationStyle = UIModalPresentationStyle.OverFullScreen,
                ModalTransitionStyle = UIModalTransitionStyle.CrossDissolve
            };
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            View.BackgroundColor = UIColor.Cyan;

            var set = CreateBindingSet();
            set.Bind(btnTabs).To(vm => vm.ShowTabsCommand);
            set.Bind(btnClose).To(vm => vm.CloseCommand);
            set.Apply();
        }
    }
}
