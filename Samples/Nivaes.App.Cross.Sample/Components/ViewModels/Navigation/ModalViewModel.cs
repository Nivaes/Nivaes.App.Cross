// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Commands;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;
    using Nivaes.App.Cross.ViewModels;

    public class ModalViewModel
        : MvxNavigationViewModel
    {
        public ModalViewModel(/*IMvxLogProvider logProvider,*/ IMvxNavigationService navigationService)
            : base(/*logProvider,*/ navigationService)
        {
            ShowTabsCommand = new MvxCommandAsync(async () => await NavigationService.Navigate<TabsRootViewModel>().ConfigureAwait(true));

            CloseCommand = new MvxCommandAsync(async () => await NavigationService.Close(this).ConfigureAwait(true));

            ShowNestedModalCommand = new MvxCommandAsync(async () => await NavigationService.Navigate<NestedModalViewModel>().ConfigureAwait(true));
        }

        public override ValueTask Initialize()
        {
            return base.Initialize();
        }

        public override ValueTask Start()
        {
            return base.Start();
        }

        public IMvxCommandAsync ShowTabsCommand { get; private set; }

        public IMvxCommandAsync CloseCommand { get; private set; }

        public IMvxCommandAsync ShowNestedModalCommand { get; private set; }
    }
}
