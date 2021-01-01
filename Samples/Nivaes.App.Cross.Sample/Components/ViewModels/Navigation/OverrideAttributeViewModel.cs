// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using Nivaes.App.Cross.ViewModels;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Commands;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;

    public class OverrideAttributeViewModel
        : MvxNavigationViewModel
    {
        public OverrideAttributeViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            CloseCommand = new MvxCommandAsync(async () => await NavigationService.Close(this).ConfigureAwait(true));

            ShowTabsCommand = new MvxCommandAsync(async () => await NavigationService.Navigate<TabsRootViewModel>().ConfigureAwait(true));

            ShowSecondChildCommand = new MvxCommandAsync(async () => await NavigationService.Navigate<SecondChildViewModel>().ConfigureAwait(true));
        }

        public IMvxCommandAsync ShowTabsCommand { get; }

        public IMvxCommandAsync CloseCommand { get; }

        public IMvxCommandAsync ShowSecondChildCommand { get; }
    }
}
