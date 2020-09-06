// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Mobile.Sample
{
    using System.Threading.Tasks;
    using MvvmCross.Commands;
    using MvvmCross.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;

    public class SheetViewModel
        : MvxNavigationViewModel
    {
        public SheetViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            CloseCommand = new MvxAsyncCommand(async () => await CloseSheet().ConfigureAwait(true));
        }

        public IMvxAsyncCommand CloseCommand { get; private set; }

        private async ValueTask CloseSheet()
        {
            _ = await NavigationService.Close(this).ConfigureAwait(true);
        }
    }
}
