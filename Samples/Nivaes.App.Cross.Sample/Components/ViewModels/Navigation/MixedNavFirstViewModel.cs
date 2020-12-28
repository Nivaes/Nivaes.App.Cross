// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using System.Threading.Tasks;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross.Commands;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;

    public class MixedNavFirstViewModel
        : MvxNavigationViewModel
    {
        public MixedNavFirstViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }

        public IMvxCommandAsync LoginCommand => new MvxCommandAsync(GotoMasterDetailPage, CanLogin);

        private bool CanLogin()
        {
            return true;
        }

        private async ValueTask<bool> GotoMasterDetailPage()
        {
            await NavigationService.Navigate<MixedNavMasterDetailViewModel>().ConfigureAwait(true);
            await NavigationService.Navigate<MixedNavMasterRootContentViewModel>().ConfigureAwait(true);

            return true;
        }
    }
}
