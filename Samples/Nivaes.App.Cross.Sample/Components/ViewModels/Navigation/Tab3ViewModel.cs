// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using Nivaes.App.Cross.Commands;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.ViewModels;

    public class Tab3ViewModel
        : MvxNavigationViewModel
    {
        public Tab3ViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            ShowRootViewModelCommand = new MvxCommandAsync(async () => await NavigationService.Navigate<RootViewModel>().ConfigureAwait(true));

            CloseViewModelCommand = new MvxCommandAsync(async () => await NavigationService.Close(this).ConfigureAwait(true));

            ShowPageOneCommand = new MvxCommandAsync(async () => await NavigationService.ChangePresentation(new MvxPagePresentationHint(typeof(Tab1ViewModel))).ConfigureAwait(true));
        }

        public IMvxCommandAsync ShowRootViewModelCommand { get; }

        public IMvxCommandAsync CloseViewModelCommand { get; }

        public IMvxCommand ShowPageOneCommand { get; }
    }
}
