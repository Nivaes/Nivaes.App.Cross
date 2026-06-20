using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class SecondChildViewModel 
    : CrossNavigationViewModel
{
    public SecondChildViewModel(ILogger<SecondChildViewModel> logger, ICrossNavigationService navigationService)
        : base(logger, navigationService)
    {
        ShowNestedChildCommand = new CrossAsyncCommand(() => NavigationService.Navigate<NestedChildViewModel>());

        CloseCommand = new CrossAsyncCommand(() => NavigationService.Close(this));
    }

    public ICrossAsyncCommand ShowNestedChildCommand { get; }

    public ICrossAsyncCommand CloseCommand { get; }
}
