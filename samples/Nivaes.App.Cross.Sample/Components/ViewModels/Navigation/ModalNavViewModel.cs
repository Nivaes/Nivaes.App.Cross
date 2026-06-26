using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class ModalNavViewModel
    : CrossNavigationViewModel
{
    public ModalNavViewModel(ICrossNavigationService navigationService, ILogger<ModalNavViewModel> logger)
        : base(navigationService, logger)
    {
        CloseCommand = new CrossAsyncCommand(() => NavigationService.Close(this));

        ShowChildCommand = new CrossAsyncCommand(() => NavigationService.Navigate<ChildViewModel>());

        ShowNestedModalCommand = new CrossAsyncCommand(() => NavigationService.Navigate<NestedModalViewModel>());
    }

    public ICrossAsyncCommand CloseCommand { get; private set; }

    public ICrossAsyncCommand ShowChildCommand { get; private set; }

    public ICrossAsyncCommand ShowNestedModalCommand { get; private set; }
}
