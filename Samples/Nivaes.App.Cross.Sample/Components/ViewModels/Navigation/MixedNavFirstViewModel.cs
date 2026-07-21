using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class MixedNavFirstViewModel : CrossNavigationViewModel
{
    public MixedNavFirstViewModel(ILogger<MixedNavFirstViewModel> logger, CrossNavigationService navigationService)
        : base(navigationService, logger)
    {
    }

    public ICrossAsyncCommand LoginCommand => new CrossAsyncCommand(GotoMasterDetailPage, CanLogin);

    private bool CanLogin()
    {
        return true;
    }

    private async Task GotoMasterDetailPage()
    {
        await NavigationService.Navigate<MixedNavMasterDetailViewModel>();
        await NavigationService.Navigate<MixedNavMasterRootContentViewModel>();
    }
}
