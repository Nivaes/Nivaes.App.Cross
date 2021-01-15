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

    public class TabsRootViewModel
        : MvxNavigationViewModel
    {
        public TabsRootViewModel(/*IMvxLogProvider logProvider,*/ IMvxNavigationService navigationService)
            : base(/*logProvider,*/ navigationService)
        {
            ShowInitialViewModelsCommand = new MvxCommandAsync(ShowInitialViewModels);
            ShowTabsRootBCommand = new MvxCommandAsync(() => NavigationService.Navigate<TabsRootBViewModel>());
        }

        public IMvxCommandAsync ShowInitialViewModelsCommand { get; }

        public IMvxCommandAsync ShowTabsRootBCommand { get; }

        private async ValueTask<bool> ShowInitialViewModels()
        {
            var tasks = new[]
            {
                NavigationService.Navigate<Tab1ViewModel, string>("test").AsTask(),
                NavigationService.Navigate<Tab2ViewModel>().AsTask(),
                NavigationService.Navigate<Tab3ViewModel>().AsTask()
            };
            await Task.WhenAll(tasks).ConfigureAwait(false);

            return true;
        }

        private int mItemIndex;

        public int ItemIndex
        {
            get => mItemIndex;
            set
            {
                if (mItemIndex == value) return;
                mItemIndex = value;
                //Log.Trace($"Tab item changed to {mItemIndex}");
                RaisePropertyChanged(() => ItemIndex);
            }
        }
    }
}
