// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross.Commands;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;

    public class SecondChildViewModel : MvxNavigationViewModel
    {
        public SecondChildViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            ShowNestedChildCommand = new MvxCommandAsync(async () => await NavigationService.Navigate<NestedChildViewModel>());

            CloseCommand = new MvxCommandAsync(async () => await NavigationService.Close(this));
        }

        public IMvxCommandAsync ShowNestedChildCommand { get; private set; }

        public IMvxCommandAsync CloseCommand { get; private set; }
    }
}
