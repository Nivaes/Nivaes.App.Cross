﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using Nivaes.App.Cross.Commands;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;
    using Nivaes.App.Cross.ViewModels;

    public class SplitDetailViewModel
        : MvxNavigationViewModel
    {
        public SplitDetailViewModel(/*IMvxLogProvider logProvider,*/ IMvxNavigationService navigationService)
            : base(/*logProvider,*/ navigationService)
        {
            ShowChildCommand = new MvxAsyncNavigationCommand<SplitDetailNavViewModel>();
            ShowTabsCommand = new MvxAsyncNavigationCommand<TabsRootBViewModel>();
            ShowTabbedChildCommand = new MvxAsyncNavigationCommand<TabsRootViewModel>();
        }

        public IMvxCommandAsync ShowChildCommand { get; }
        public IMvxCommandAsync ShowTabsCommand { get; }
        public IMvxCommandAsync ShowTabbedChildCommand { get; }

        public string ContentText => "Text for the Content Area";
    }
}
