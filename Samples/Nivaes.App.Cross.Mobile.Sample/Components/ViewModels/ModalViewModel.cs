// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Mobile.Sample
{
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class ModalViewModel
        : MvxNavigationViewModel
    {
        public ModalViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            ShowTabsCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<TabsRootViewModel>().ConfigureAwait(true));

            CloseCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this).ConfigureAwait(true));

            ShowNestedModalCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<NestedModalViewModel>().ConfigureAwait(true));
        }

        public override ValueTask Initialize()
        {
            return base.Initialize();
        }

        public override ValueTask Start()
        {
            return base.Start();
        }

        public IMvxAsyncCommand ShowTabsCommand { get; private set; }

        public IMvxAsyncCommand CloseCommand { get; private set; }

        public IMvxAsyncCommand ShowNestedModalCommand { get; private set; }
    }
}
