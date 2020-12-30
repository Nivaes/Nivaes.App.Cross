﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross.Commands;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;

    public class NestedModalViewModel
        : MvxNavigationViewModel
    {
        public NestedModalViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            CloseCommand = new MvxCommandAsync(async () => await NavigationService.Close(this).ConfigureAwait(true));

            ShowTabsCommand = new MvxCommandAsync(async () => await NavigationService.Navigate<TabsRootViewModel>().ConfigureAwait(true));
        }

        public IMvxCommandAsync ShowTabsCommand { get; private set; }

        public IMvxCommandAsync CloseCommand { get; private set; }
    }
}