// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;

    public class ModalNavViewModel
        : MvxNavigationViewModel
    {
        public ModalNavViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            CloseCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this).ConfigureAwait(true));

            ShowChildCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<ChildViewModel>().ConfigureAwait(true));

            ShowNestedModalCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<NestedModalViewModel>().ConfigureAwait(true));
        }

        public IMvxAsyncCommand CloseCommand { get; private set; }

        public IMvxAsyncCommand ShowChildCommand { get; private set; }

        public IMvxAsyncCommand ShowNestedModalCommand { get; private set; }
    }
}
