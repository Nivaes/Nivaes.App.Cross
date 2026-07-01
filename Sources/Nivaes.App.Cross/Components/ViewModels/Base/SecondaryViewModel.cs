using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public sealed class SecondaryViewModel
        : BaseMainViewModel
    {
        public SecondaryViewModel(ICrossNavigationService navigationService, ILogger logger)
          : base(navigationService, logger)
        { }
    }
}
