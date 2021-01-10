// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using Nivaes.App.Cross.Commands;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;
    using Nivaes.App.Cross.ViewModels;

    public class Tab2ViewModel : MvxNavigationViewModel
    {
        public Tab2ViewModel(/*IMvxLogProvider logProvider, */IMvxNavigationService navigationService)
            : base(/*logProvider,*/ navigationService)
        {
            ShowRootViewModelCommand = new MvxCommandAsync(async () => await NavigationService.Navigate<RootViewModel>().ConfigureAwait(true));

            CloseViewModelCommand = new MvxCommandAsync(async () => await NavigationService.Close(this).ConfigureAwait(true));
        }

        public IMvxCommandAsync ShowRootViewModelCommand { get; private set; }

        public IMvxCommandAsync CloseViewModelCommand { get; private set; }
    }
}
