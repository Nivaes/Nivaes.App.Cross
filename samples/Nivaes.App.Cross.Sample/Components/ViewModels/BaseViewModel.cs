using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.Sample;

public class BaseViewModel 
    : CrossNavigationViewModel
{
    public BaseViewModel(ILoggerFactory loggerFactory, ICrossNavigationService navigationService)
        : base(loggerFactory, navigationService)
    {
    }
}
