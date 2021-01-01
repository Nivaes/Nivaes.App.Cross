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

    public class SplitRootViewModel
        : MvxNavigationViewModel
    {
        public SplitRootViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            ShowInitialMenuCommand = new MvxCommandAsync(
                async () => await ShowInitialViewModel().ConfigureAwait(true));
            ShowDetailCommand = new MvxCommandAsync(
                async () => await ShowDetailViewModel().ConfigureAwait(true));
        }

        public IMvxCommandAsync ShowInitialMenuCommand { get; }

        public IMvxCommandAsync ShowDetailCommand { get; }

        public override ValueTask ViewAppeared()
        {
            return new ValueTask(Task.WhenAll(ShowInitialViewModel().AsTask(), ShowDetailViewModel().AsTask()));
        }

        private ValueTask<bool> ShowInitialViewModel()
        {
            return NavigationService.Navigate<SplitMasterViewModel>();
        }

        private ValueTask<bool> ShowDetailViewModel()
        {
            return NavigationService.Navigate<SplitDetailViewModel>();
        }
    }
}
