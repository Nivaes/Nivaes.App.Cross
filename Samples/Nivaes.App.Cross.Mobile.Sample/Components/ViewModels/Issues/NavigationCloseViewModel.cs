// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Mobile.Sample
{
    using System.Threading.Tasks;
    using MvvmCross;
    using MvvmCross.Commands;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;

    public class NavigationCloseViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService mMvxNavigationService;

        public NavigationCloseViewModel(IMvxNavigationService mvxNavigationService)
        {
            mMvxNavigationService = mvxNavigationService;
        }

        public IMvxAsyncCommand OpenChildThenCloseThisCommand => new MvxAsyncCommand(CloseThisAndOpenChildAsync);

        public IMvxAsyncCommand TryToCloseNewViewModelCommand => new MvxAsyncCommand(TryToCloseNewViewModelAsync);

        private async Task CloseThisAndOpenChildAsync()
        {
            await mMvxNavigationService.Navigate<SecondChildViewModel>().ConfigureAwait(false);
            await mMvxNavigationService.Close(this).ConfigureAwait(false);
        }

        private async Task TryToCloseNewViewModelAsync()
        {
            await mMvxNavigationService.Close(Mvx.IoCProvider.Resolve<SecondChildViewModel>());
        }
    }
}
