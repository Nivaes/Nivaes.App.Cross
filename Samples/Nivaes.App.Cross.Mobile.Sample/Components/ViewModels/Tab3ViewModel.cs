﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Mobile.Sample
{
    using MvvmCross.Commands;
    using MvvmCross.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.Presenters.Hints;
    using MvvmCross.ViewModels;

    public class Tab3ViewModel
        : MvxNavigationViewModel
    {
        public Tab3ViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            ShowRootViewModelCommand = new MvxAsyncCommand(async () => await NavigationService.Navigate<RootViewModel>().ConfigureAwait(true));

            CloseViewModelCommand = new MvxAsyncCommand(async () => await NavigationService.Close(this).ConfigureAwait(true));

            ShowPageOneCommand = new MvxCommand(() => NavigationService.ChangePresentation(new MvxPagePresentationHint(typeof(Tab1ViewModel))));
        }

        public IMvxAsyncCommand ShowRootViewModelCommand { get; private set; }

        public IMvxAsyncCommand CloseViewModelCommand { get; private set; }

        public IMvxCommand ShowPageOneCommand { get; private set; }
    }
}