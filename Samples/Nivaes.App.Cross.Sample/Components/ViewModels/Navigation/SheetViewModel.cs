// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using System.Threading.Tasks;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross.Commands;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;

    public class SheetViewModel
        : MvxNavigationViewModel
    {
        public SheetViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            CloseCommand = new MvxCommandAsync(CloseSheet);
        }

        public IMvxCommandAsync CloseCommand { get; private set; }

        private ValueTask<bool> CloseSheet()
        {
            return NavigationService.Close(this);
        }
    }
}
