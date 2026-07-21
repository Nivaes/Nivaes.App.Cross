using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class TabsRootViewModel
    : CrossNavigationViewModel
{
    public TabsRootViewModel(ILogger<TabsRootViewModel> logger)
        : base(logger)
    {
        ShowInitialViewModelsCommand = new CrossAsyncCommand(ShowInitialViewModels);
        ShowTabsRootBCommand = new CrossAsyncCommand(() => NavigationService.Navigate<TabsRootBViewModel>());
    }

    public ICrossAsyncCommand ShowInitialViewModelsCommand { get; }

    public ICrossAsyncCommand ShowTabsRootBCommand { get; }

    private Task ShowInitialViewModels()
    {
        var tasks = new List<Task>();
        tasks.Add(NavigationService.Navigate<Tab1ViewModel, string>("test"));
        tasks.Add(NavigationService.Navigate<Tab2ViewModel>());
        tasks.Add(NavigationService.Navigate<Tab3ViewModel>());
        return Task.WhenAll(tasks);
    }

    private int _itemIndex;

    public int ItemIndex
    {
        get { return _itemIndex; }
        set
        {
            if (_itemIndex == value) return;
            _itemIndex = value;
            Logger.LogTrace("Tab item changed to {ItemIndex}", _itemIndex);
            RaisePropertyChanged(() => ItemIndex);
        }
    }
}
