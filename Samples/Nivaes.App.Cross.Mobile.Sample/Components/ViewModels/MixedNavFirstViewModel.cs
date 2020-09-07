﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Mobile.Sample
{
    using System.Threading.Tasks;
    using MvvmCross.Commands;
    using MvvmCross.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;

    public class MixedNavFirstViewModel
        : MvxNavigationViewModel
    {
        public MixedNavFirstViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }

        public IMvxAsyncCommand LoginCommand => new MvxAsyncCommand(
            async ()=> await GotoMasterDetailPage().ConfigureAwait(true), 
            CanLogin);

        private bool CanLogin()
        {
            return true;
        }

        private async Task GotoMasterDetailPage()
        {
            await NavigationService.Navigate<MixedNavMasterDetailViewModel>().ConfigureAwait(true);
            await NavigationService.Navigate<MixedNavMasterRootContentViewModel>().ConfigureAwait(true);
        }
    }
}