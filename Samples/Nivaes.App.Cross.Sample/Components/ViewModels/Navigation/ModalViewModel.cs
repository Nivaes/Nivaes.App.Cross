using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class ModalViewModel
    : CrossNavigationViewModel
{
    public ModalViewModel(ILogger<ModalViewModel> logger, ICrossNavigationService navigationService)
        : base(navigationService, logger)
    {
        ShowTabsCommand = new CrossAsyncCommand(() => NavigationService.Navigate<TabsRootViewModel>());

        CloseCommand = new CrossAsyncCommand(() => NavigationService.Close(this));

        ShowNestedModalCommand = new CrossAsyncCommand(() => NavigationService.Navigate<NestedModalViewModel>());
    }

    public ICrossAsyncCommand ShowTabsCommand { get; }

    public ICrossAsyncCommand CloseCommand { get; }

    public ICrossAsyncCommand ShowNestedModalCommand { get; }
}
