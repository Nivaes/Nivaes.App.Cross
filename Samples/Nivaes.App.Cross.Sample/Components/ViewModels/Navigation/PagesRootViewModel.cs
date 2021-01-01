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

    public class PagesRootViewModel
        : MvxNavigationViewModel
    {
        public PagesRootViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            ShowInitialViewModelsCommand = new MvxCommandAsync(ShowInitialViewModels);
        }

        public IMvxCommandAsync ShowInitialViewModelsCommand { get; private set; }

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
