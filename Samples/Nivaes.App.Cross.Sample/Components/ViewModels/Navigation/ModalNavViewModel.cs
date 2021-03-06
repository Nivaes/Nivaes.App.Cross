﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using Nivaes.App.Cross.Commands;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;
    using Nivaes.App.Cross.ViewModels;

    public class ModalNavViewModel
        : MvxNavigationViewModel
    {
        public ModalNavViewModel(/*IMvxLogProvider logProvider,*/ IMvxNavigationService navigationService)
            : base(/*logProvider,*/ navigationService)
        {
            CloseCommand = new MvxCommandAsync(async () => await NavigationService.Close(this).ConfigureAwait(true));

            ShowChildCommand = new MvxCommandAsync(async () => await NavigationService.Navigate<ChildViewModel>().ConfigureAwait(true));

            ShowNestedModalCommand = new MvxCommandAsync(async () => await NavigationService.Navigate<NestedModalViewModel>().ConfigureAwait(true));
        }

        public IMvxCommandAsync CloseCommand { get; private set; }

        public IMvxCommandAsync ShowChildCommand { get; private set; }

        public IMvxCommandAsync ShowNestedModalCommand { get; private set; }
    }
}
