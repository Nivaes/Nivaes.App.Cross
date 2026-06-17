using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Playground.Core.ViewModels;

namespace Nivaes.App.Cross.Sample;

public class TabsRootViewModel 
    : CrossNavigationViewModel
{
    public TabsRootViewModel(ILoggerFactory logProvider, ICrossNavigationService navigationService) 
        : base(logProvider, navigationService)
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
            Log.LogTrace("Tab item changed to {ItemIndex}", _itemIndex);
            RaisePropertyChanged(() => ItemIndex);
        }
    }
}
