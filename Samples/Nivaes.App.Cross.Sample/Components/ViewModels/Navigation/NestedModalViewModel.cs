using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class NestedModalViewModel : CrossNavigationViewModel
{
    public NestedModalViewModel(ILogger<NestedModalViewModel> logger)
        : base(logger)
    {
        CloseCommand = new CrossAsyncCommand(() => NavigationService.Close(this));

        ShowTabsCommand = new CrossAsyncCommand(() => NavigationService.Navigate<TabsRootViewModel>());
    }

    public ICrossAsyncCommand ShowTabsCommand { get; }

    public ICrossAsyncCommand CloseCommand { get; }
}
