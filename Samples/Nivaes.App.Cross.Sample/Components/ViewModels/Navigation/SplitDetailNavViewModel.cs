using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class SplitDetailNavViewModel
    : CrossNavigationViewModel
{
    public SplitDetailNavViewModel(ILogger<SplitDetailNavViewModel> logger, CrossNavigationService navigationService)
        : base(navigationService, logger)
    {
        MainMenuCommand = new CrossAsyncCommand(() => NavigationService.Navigate<MixedNavFirstViewModel>());
        CloseCommand = new CrossAsyncCommand(() => NavigationService.Close(this));
    }

    public ICrossAsyncCommand MainMenuCommand { get; }
    public ICrossAsyncCommand CloseCommand { get; }

}
