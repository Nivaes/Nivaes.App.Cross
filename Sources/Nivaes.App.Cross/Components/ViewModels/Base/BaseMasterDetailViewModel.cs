using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public abstract class BaseMasterDetailViewModel
        : BaseMainViewModel
    {
        protected BaseMasterDetailViewModel(ICrossNavigationService navigationService, ILogger logger)
            : base(navigationService, logger)
        { }
    }
}
