// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using System.Threading.Tasks;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Commands;
    using Nivaes.App.Cross.Navigation;
    using Nivaes.App.Cross.ViewModels;

    public class NavigationCloseViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService mMvxNavigationService;

        public NavigationCloseViewModel(IMvxNavigationService mvxNavigationService)
        {
            mMvxNavigationService = mvxNavigationService;
        }

        public IMvxCommandAsync OpenChildThenCloseThisCommand => new MvxCommandAsync(CloseThisAndOpenChild);

        public IMvxCommandAsync TryToCloseNewViewModelCommand => new MvxCommandAsync(TryToCloseNewViewModel);

        private async ValueTask<bool> CloseThisAndOpenChild()
        {
            await mMvxNavigationService.Navigate<SecondChildViewModel>().ConfigureAwait(false);
            return await mMvxNavigationService.Close(this).ConfigureAwait(false);
        }

        private ValueTask<bool> TryToCloseNewViewModel()
        {
            return mMvxNavigationService.Close(Mvx.IoCProvider.Resolve<SecondChildViewModel>());
        }
    }
}
