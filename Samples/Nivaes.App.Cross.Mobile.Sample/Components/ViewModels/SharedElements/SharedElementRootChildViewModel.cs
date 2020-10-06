// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Mobile.Sample
{
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;

    public class SharedElementRootChildViewModel
        : MvxNavigationViewModel
    {
        public override ValueTask Initialize()
        {
            Items = new MvxObservableCollection<ListItemViewModel>
            {
                new ListItemViewModel { Id = 1, Title = "title one Fragment" },
                new ListItemViewModel { Id = 2, Title = "title two Activity" },
                new ListItemViewModel { Id = 3, Title = "title three Fragment" },
                new ListItemViewModel { Id = 4, Title = "title four Activity" },
                new ListItemViewModel { Id = 5, Title = "title five Fragment" }
            };

            return base.Initialize();
        }

        private MvxObservableCollection<ListItemViewModel>? mItems;
        public MvxObservableCollection<ListItemViewModel>? Items
        {
            get => mItems;
            set => SetProperty(ref mItems, value);
        }

        private ListItemViewModel mSelectedItem;

        public ListItemViewModel SelectedItem
        {
            get => mSelectedItem;
            set => SetProperty(ref mSelectedItem, value);
        }

        public SharedElementRootChildViewModel(IMvxLogProvider logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }

        public void SelectItemExecution(ListItemViewModel item)
        {
            SelectedItem = item;

            if (item.Id % 2 == 0)
            {
                NavigationService.Navigate<SharedElementSecondViewModel>();
            }
            else
            {
                NavigationService.Navigate<SharedElementSecondChildViewModel>();
            }
        }
    }
}
