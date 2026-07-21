using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

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

    #region Items
    private CrossObservableCollection<ListItemViewModel>? _items;
    public CrossObservableCollection<ListItemViewModel>? Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }
    #endregion

    #region SelectedItem
    private ListItemViewModel? _selectedItem;

    public ListItemViewModel? SelectedItem
    {
        get => _selectedItem;
        set => SetProperty(ref _selectedItem, value);
    }
    #endregion

    public SharedElementRootChildViewModel(ILogger<SharedElementRootChildViewModel> logger, CrossNavigationService navigationService)
        : base(logger, navigationService)
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
