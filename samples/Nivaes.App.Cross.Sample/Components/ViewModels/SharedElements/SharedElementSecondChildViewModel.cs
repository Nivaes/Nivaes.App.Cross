using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class SharedElementSecondChildViewModel
    : BaseViewModel
{
    public SharedElementSecondChildViewModel(ILogger<SharedElementSecondChildViewModel> logger, ICrossNavigationService navigationService)
        : base(logger, navigationService)
    {
    }
}
