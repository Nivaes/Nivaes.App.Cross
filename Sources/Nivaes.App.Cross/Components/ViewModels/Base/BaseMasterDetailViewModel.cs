using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public abstract class BaseMasterDetailViewModel
        : BaseMainViewModel
    {
        protected BaseMasterDetailViewModel(CrossNavigationService navigationService, ILogger logger)
            : base(navigationService, logger)
        { }
    }
}
