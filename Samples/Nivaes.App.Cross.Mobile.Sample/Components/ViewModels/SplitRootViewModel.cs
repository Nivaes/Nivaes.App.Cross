﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Mobile.Sample
{
    using System.Threading.Tasks;
    using MvvmCross.Commands;
    using MvvmCross.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;

    public class SplitRootViewModel : MvxNavigationViewModel
    {
        public SplitRootViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            ShowInitialMenuCommand = new MvxAsyncCommand(
                async () => await ShowInitialViewModel().ConfigureAwait(true));
            ShowDetailCommand = new MvxAsyncCommand(
                async () => await ShowDetailViewModel().ConfigureAwait(true));
        }

        public IMvxAsyncCommand ShowInitialMenuCommand { get; private set; }

        public IMvxAsyncCommand ShowDetailCommand { get; private set; }

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
