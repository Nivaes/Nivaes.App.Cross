// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Sample
{
    using System.Threading.Tasks;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross.Navigation;

    public class PagesRootViewModel
        : MvxNavigationViewModel
    {
        public PagesRootViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            ShowInitialViewModelsCommand = new MvxAsyncCommand(ShowInitialViewModels);
        }

        public IMvxAsyncCommand ShowInitialViewModelsCommand { get; private set; }

        private async ValueTask<bool> ShowInitialViewModels()
        {
            var tasks = new[]
            {
                NavigationService.Navigate<Page1ViewModel>().AsTask(),
                NavigationService.Navigate<Page2ViewModel>().AsTask(),
                NavigationService.Navigate<Page3ViewModel>().AsTask()
            };
            await Task.WhenAll(tasks).ConfigureAwait(false);

            return true;
        }
    }
}
