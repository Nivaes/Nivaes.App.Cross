// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross.Commands;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;
    using Nivaes.App.Cross.Presenters;

    public class Tab3ViewModel
        : MvxNavigationViewModel
    {
        public Tab3ViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            ShowRootViewModelCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<RootViewModel>().ConfigureAwait(true));

            CloseViewModelCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this).ConfigureAwait(true));

            ShowPageOneCommand = new MvxAsyncCommand(async () => await NavigationService.ChangePresentation(new MvxPagePresentationHint(typeof(Tab1ViewModel))).ConfigureAwait(true));
        }

        public IMvxAsyncCommand ShowRootViewModelCommand { get; }

        public IMvxAsyncCommand CloseViewModelCommand { get; }

        public IMvxCommand ShowPageOneCommand { get; }
    }
}
