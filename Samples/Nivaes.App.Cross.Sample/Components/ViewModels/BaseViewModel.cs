using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class BaseViewModel
    : CrossNavigationViewModel
{
    public BaseViewModel(ILogger<BaseViewModel> logger, CrossNavigationService navigationService)
        : base(navigationService, logger)
    {
    }
}
