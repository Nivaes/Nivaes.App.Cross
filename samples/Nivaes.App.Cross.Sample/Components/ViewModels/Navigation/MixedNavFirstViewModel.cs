using Microsoft.Extensions.Logging;
using Playground.Core.ViewModels;

namespace Nivaes.App.Cross.Sample;

public class MixedNavFirstViewModel : CrossNavigationViewModel
{
    public MixedNavFirstViewModel(ILoggerFactory logProvider, ICrossNavigationService navigationService)
        : base(logProvider, navigationService)
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
