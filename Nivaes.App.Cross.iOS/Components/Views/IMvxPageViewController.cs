// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Ios.Views
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross.Presenters;
    using UIKit;

    public interface IMvxPageViewController
    {
        void AddPage(UIViewController viewController, MvxPagePresentationAttribute attribute);

        bool RemovePage(IMvxViewModel viewModel);
    }
}
