using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class SplitDetailViewModel 
    : CrossNavigationViewModel
{
    public SplitDetailViewModel(ILoggerFactory logProvider, ICrossNavigationService navigationService)
        : base(logProvider, navigationService)
    {
        ShowChildCommand = new CrossAsyncCommand(() => NavigationService.Navigate<SplitDetailNavViewModel>());
        ShowTabsCommand = new CrossAsyncCommand(() => NavigationService.Navigate<TabsRootBViewModel>());
        ShowTabbedChildCommand = new CrossAsyncCommand(() => NavigationService.Navigate<TabsRootViewModel>());
    }

    public ICrossAsyncCommand ShowChildCommand { get; }
    public ICrossAsyncCommand ShowTabsCommand { get; }
    public ICrossAsyncCommand ShowTabbedChildCommand { get; }

    public string ContentText => "Text for the Content Area";
}
