using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public sealed class PrimaryMasterDetailViewModel
        : BaseMasterDetailViewModel
    {
        public PrimaryMasterDetailViewModel(ICrossNavigationService navigationService, ILogger logger)
            : base(navigationService, logger)
        { }
    }
}
