using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class SecondChildViewModel 
    : CrossNavigationViewModel
{
    public SecondChildViewModel(ILoggerFactory logFactory, ICrossNavigationService navigationService)
        : base(logFactory, navigationService)
    {
        ShowNestedChildCommand = new CrossAsyncCommand(() => NavigationService.Navigate<NestedChildViewModel>());

        CloseCommand = new CrossAsyncCommand(() => NavigationService.Close(this));
    }

    public ICrossAsyncCommand ShowNestedChildCommand { get; }

    public ICrossAsyncCommand CloseCommand { get; }
}
