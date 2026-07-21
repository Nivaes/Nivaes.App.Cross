using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class SplitMasterViewModel
    : CrossNavigationViewModel
{
    public SplitMasterViewModel(ILogger<SplitMasterViewModel> logger, CrossNavigationService navigationService)
        : base(navigationService, logger)
    {
        OpenDetailCommand = new CrossAsyncCommand(() => NavigationService.Navigate<SplitDetailViewModel>());

        OpenDetailNavCommand = new CrossAsyncCommand(() => NavigationService.Navigate<SplitDetailNavViewModel>());

        ShowRootViewModel = new CrossAsyncCommand(() => NavigationService.Navigate<RootViewModel>());
    }

    public string PaneText => "Text for the Master Pane";

    public ICrossAsyncCommand OpenDetailCommand { get; }

    public ICrossAsyncCommand OpenDetailNavCommand { get; }

    public ICrossAsyncCommand ShowRootViewModel { get; }
}
