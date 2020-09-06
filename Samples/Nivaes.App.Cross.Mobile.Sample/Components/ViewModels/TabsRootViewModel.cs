// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Mobile.Sample
{
    using System.Threading.Tasks;
    using MvvmCross.Commands;
    using MvvmCross.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;

    public class TabsRootViewModel : MvxNavigationViewModel
    {
        public TabsRootViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService) : base(logProvider, navigationService)
        {
            ShowInitialViewModelsCommand = new MvxAsyncCommand(
                async () => await ShowInitialViewModels().ConfigureAwait(false));
            ShowTabsRootBCommand = new MvxAsyncCommand(
                async () => await NavigationService.Navigate<TabsRootBViewModel>().ConfigureAwait(false));
        }

        public IMvxAsyncCommand ShowInitialViewModelsCommand { get; private set; }

        public IMvxAsyncCommand ShowTabsRootBCommand { get; private set; }

        private Task ShowInitialViewModels()
        {
            var tasks = new[]
            {
                NavigationService.Navigate<Tab1ViewModel, string>("test"),
                NavigationService.Navigate<Tab2ViewModel>(),
                NavigationService.Navigate<Tab3ViewModel>()
            };
            return Task.WhenAll(tasks);
        }

        private int mItemIndex;

        public int ItemIndex
        {
            get => mItemIndex;
            set
            {
                if (mItemIndex == value) return;
                mItemIndex = value;
                Log.Trace($"Tab item changed to {mItemIndex}");
                RaisePropertyChanged(() => ItemIndex);
            }
        }
    }
}
