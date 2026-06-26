using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class SharedElementRootViewModel
    : BaseViewModel
{
    public SharedElementRootViewModel(ILogger<SharedElementRootViewModel> logger, ICrossNavigationService navigationService)
        : base(logger, navigationService)
    {
    }
}
