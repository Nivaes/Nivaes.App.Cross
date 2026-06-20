using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class RegionViewModel 
    : CrossNavigationViewModel
{
    public RegionViewModel(ILogger<RegionViewModel> logger, ICrossNavigationService navigationService) 
        : base(logger, navigationService)
    {
    }

    public ICrossAsyncCommand CloseRegionCommand =>
        new CrossAsyncCommand(() => this.NavigationService.Close(this));
}
