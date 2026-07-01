using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public sealed class SecondaryMasterDetailViewModel
        : BaseMasterDetailViewModel
    {
        public SecondaryMasterDetailViewModel(ICrossNavigationService navigationService, ILogger logger)
            : base(navigationService, logger)
        { }
    }
}
