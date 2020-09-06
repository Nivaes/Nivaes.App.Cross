﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Mobile.Sample
{
    using MvvmCross.Commands;
    using MvvmCross.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;

    public class SplitDetailNavViewModel : MvxNavigationViewModel
    {
        public SplitDetailNavViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            MainMenuCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<MixedNavFirstViewModel>().ConfigureAwait(true));
            CloseCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this).ConfigureAwait(true));
        }

        public IMvxAsyncCommand MainMenuCommand { get; private set; }
        public IMvxAsyncCommand CloseCommand { get; private set; }

    }
}
