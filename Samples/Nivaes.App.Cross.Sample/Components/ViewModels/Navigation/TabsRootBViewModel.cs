using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class TabsRootBViewModel
    : CrossNavigationViewModel
{
    public TabsRootBViewModel(ILogger<TabsRootBViewModel> logger)
        : base(logger)
    {
        ShowInitialViewModelsCommand = new CrossAsyncCommand(ShowInitialViewModels);
    }

    public ICrossAsyncCommand ShowInitialViewModelsCommand { get; }

    private Task ShowInitialViewModels()
    {
        var tasks = new List<Task>();
        tasks.Add(NavigationService.Navigate<Tab1ViewModel, string>("test"));
        tasks.Add(NavigationService.Navigate<Tab2ViewModel>());
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
