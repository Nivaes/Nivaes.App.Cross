using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross
{
    public sealed class MainDetailViewModel
        : BaseDetailViewModel
    {
        public MainDetailViewModel(CrossNavigationService navigationService, ILogger logger)
            : base(navigationService, logger)
        { }
    }
}
