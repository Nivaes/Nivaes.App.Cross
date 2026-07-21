using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class SharedElementSecondViewModel
    : BaseViewModel
{
    public SharedElementSecondViewModel(ILogger<SharedElementSecondChildViewModel> logger)
        : base(logger)
    {
    }
}
