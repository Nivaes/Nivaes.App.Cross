// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Mobile.iOS.Sample
{
    using System;
    using MvvmCross.Platforms.Ios.Views;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.Sample;
    using Nivaes.App.Cross.ViewModels;
    using UIKit;

    [MvxFromStoryboard("TabsRootView")]
    [MvxRootPresentation(WrapInNavigationController = true)]
    public partial class TabsRootView
        : MvxTabBarViewController<TabsRootViewModel>
    {
        private bool mIsPresentedFirstTime = true;

        public TabsRootView(IntPtr handle) : base(handle)
        {
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewWillAppear(animated);

            if (ViewModel != null && mIsPresentedFirstTime)
            {
                mIsPresentedFirstTime = false;
                ViewModel.ShowInitialViewModelsCommand.ExecuteAsync(null);
            }
        }

        protected override void SetTitleAndTabBarItem(UIViewController viewController, MvxTabPresentationAttribute attribute)
        {
            // you can override this method to set title or iconName
            if (string.IsNullOrEmpty(attribute.TabName))
                attribute.TabName = "Tab 2";
            if (string.IsNullOrEmpty(attribute.TabIconName))
                attribute.TabIconName = "ic_tabbar_menu";

            base.SetTitleAndTabBarItem(viewController, attribute);
        }

        public override bool ShowChildView(UIViewController viewController)
        {
            var type = viewController.GetType();

            return type == typeof(ChildView)
                ? false
                : base.ShowChildView(viewController);
        }

        public override bool CloseChildViewModel(IMvxViewModel viewModel)
        {
            var type = viewModel.GetType();

            return type == typeof(ChildViewModel)
                ? false
                : base.CloseChildViewModel(viewModel);
        }
    }
}
