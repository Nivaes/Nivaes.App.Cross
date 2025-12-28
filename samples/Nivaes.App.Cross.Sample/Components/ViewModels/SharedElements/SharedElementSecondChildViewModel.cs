using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class SharedElementSecondChildViewModel 
    : BaseViewModel
{
    public SharedElementSecondChildViewModel(ILoggerFactory logProvider, ICrossNavigationService navigationService)
        : base(logProvider, navigationService)
    {
    }
}
