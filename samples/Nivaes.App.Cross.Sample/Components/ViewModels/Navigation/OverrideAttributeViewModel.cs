using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class OverrideAttributeViewModel 
    : CrossNavigationViewModel
{
    public OverrideAttributeViewModel(ILoggerFactory logProvider, ICrossNavigationService navigationService)
        : base(logProvider, navigationService)
    {
        CloseCommand = new CrossAsyncCommand(() => NavigationService.Close(this));

        ShowTabsCommand = new CrossAsyncCommand(() => NavigationService.Navigate<TabsRootViewModel>());

        ShowSecondChildCommand = new CrossAsyncCommand(() => NavigationService.Navigate<SecondChildViewModel>());
    }

    public ICrossAsyncCommand ShowTabsCommand { get; }

    public ICrossAsyncCommand CloseCommand { get; }

    public ICrossAsyncCommand ShowSecondChildCommand { get; }
}
