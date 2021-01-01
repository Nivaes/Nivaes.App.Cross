// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Ios.Views
{
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.ViewModels;
    using UIKit;

    public interface IMvxSplitViewController
    {
        void ShowMasterView(UIViewController viewController, MvxSplitViewPresentationAttribute attribute);

        void ShowDetailView(UIViewController viewController, MvxSplitViewPresentationAttribute attribute);

        bool CloseChildViewModel(IMvxViewModel viewModel, MvxBasePresentationAttribute attribute);
    }
}
