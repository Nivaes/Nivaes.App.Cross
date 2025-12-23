namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    public class SharedElementRootChildViewModel 
        : BaseViewModel
    {
        public override Task Initialize()
        {
            Items = new CrossObservableCollection<ListItemViewModel>
            {
                new ListItemViewModel { Id = 1, Title = "title one Fragment" },
                new ListItemViewModel { Id = 2, Title = "title two Activity" },
                new ListItemViewModel { Id = 3, Title = "title three Fragment" },
                new ListItemViewModel { Id = 4, Title = "title four Activity" },
                new ListItemViewModel { Id = 5, Title = "title five Fragment" }
            };

            return base.Initialize();
        }

        private CrossObservableCollection<ListItemViewModel> _items;
        public CrossObservableCollection<ListItemViewModel> Items
        {
            get => _items;
            set => SetProperty(ref _items, value);
        }

        private ListItemViewModel _selectedItem;

        public SharedElementRootChildViewModel(ILoggerFactory logProvider, ICrossNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }

        public ListItemViewModel SelectedItem
        {
            get => _selectedItem;
            set => SetProperty(ref _selectedItem, value);
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
