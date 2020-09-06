﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Mobile.Sample
{
    using System.Threading.Tasks;
    using MvvmCross.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;

    public class MixedNavTabsViewModel
        : MvxNavigationViewModel
    {
        public MixedNavTabsViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }

        public override async ValueTask ViewAppearing()
        {
            await ShowInitialViewModels().ConfigureAwait(false);
            await base.ViewAppearing().ConfigureAwait(false);
        }

        private Task ShowInitialViewModels()
        {
            var tasks = new[]
            {
                NavigationService.Navigate<MixedNavTab1ViewModel>(),
                NavigationService.Navigate<MixedNavTab2ViewModel>(),
                //NavigationService.Navigate<Tab1ViewModel, string>("test").AsTask(),
                //NavigationService.Navigate<Tab2ViewModel>().AsTask(),
                //NavigationService.Navigate<Tab3ViewModel>().AsTask()
            };

            return Task.WhenAll(tasks);
        }
    }
}
