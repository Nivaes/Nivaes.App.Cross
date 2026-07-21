using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public sealed class PrimaryViewModel
        : BaseMainViewModel
    {
        public PrimaryViewModel(CrossNavigationService navigationService, ILogger logger)
          : base(navigationService, logger)
        { }
    }
}
