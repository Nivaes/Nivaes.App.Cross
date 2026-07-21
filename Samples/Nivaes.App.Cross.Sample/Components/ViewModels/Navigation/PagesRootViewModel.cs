using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class PagesRootViewModel
    : CrossNavigationViewModel
{
    public PagesRootViewModel(CrossNavigationService navigationService, ILogger<PagesRootViewModel> logger)
        : base(navigationService, logger)
    {
        ShowInitialViewModelsCommand = new CrossAsyncCommand(ShowInitialViewModels);
    }

    public ICrossAsyncCommand ShowInitialViewModelsCommand { get; }

    private Task ShowInitialViewModels()
    {
        var tasks = new List<Task>();
        tasks.Add(NavigationService.Navigate<Page1ViewModel>());
        tasks.Add(NavigationService.Navigate<Page2ViewModel>());
        tasks.Add(NavigationService.Navigate<Page3ViewModel>());
        return Task.WhenAll(tasks);
    }
}
