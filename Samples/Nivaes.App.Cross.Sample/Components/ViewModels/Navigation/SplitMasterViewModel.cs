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

    public class SplitMasterViewModel
        : MvxNavigationViewModel
    {
        public SplitMasterViewModel(/*IMvxLogProvider logProvider,*/ IMvxNavigationService navigationService)
            : base(/*logProvider,*/ navigationService)
        {
            OpenDetailCommand = new MvxCommandAsync(async () => await NavigationService.Navigate<SplitDetailViewModel>().ConfigureAwait(false));

            OpenDetailNavCommand = new MvxCommandAsync(async () => await NavigationService.Navigate<SplitDetailNavViewModel>().ConfigureAwait(false));

            ShowRootViewModel = new MvxCommandAsync(async () => await NavigationService.Navigate<RootViewModel>().ConfigureAwait(false));
        }

        public string PaneText => "Text for the Master Pane";

        public IMvxCommandAsync OpenDetailCommand { get; private set; }

        public IMvxCommandAsync OpenDetailNavCommand { get; private set; }

        public IMvxCommandAsync ShowRootViewModel { get; private set; }

        public override ValueTask ViewAppeared()
        {
            return base.ViewAppeared();
        }
    }
}
