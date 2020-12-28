// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Commands;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;

    public class SplitDetailNavViewModel : MvxNavigationViewModel
    {
        public SplitDetailNavViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            MainMenuCommand = new MvxCommandAsync(async () => await NavigationService.Navigate<MixedNavFirstViewModel>().ConfigureAwait(true));
            CloseCommand = new MvxCommandAsync(async () => await NavigationService.Close(this).ConfigureAwait(true));
        }

        public IMvxCommandAsync MainMenuCommand { get; }
        public IMvxCommandAsync CloseCommand { get; }
    }
}
