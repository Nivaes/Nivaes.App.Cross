// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;

    public class OverrideAttributeViewModel
        : MvxNavigationViewModel
    {
        public OverrideAttributeViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            CloseCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this).ConfigureAwait(true));

            ShowTabsCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<TabsRootViewModel>().ConfigureAwait(true));

            ShowSecondChildCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<SecondChildViewModel>().ConfigureAwait(true));
        }

        public IMvxAsyncCommand ShowTabsCommand { get; }

        public IMvxAsyncCommand CloseCommand { get; }

        public IMvxAsyncCommand ShowSecondChildCommand { get; }
    }
}
