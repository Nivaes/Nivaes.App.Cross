namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    public class Page3ViewModel 
        : CrossNavigationViewModel
    {
        public Page3ViewModel(ILogger<Page3ViewModel> logger, ICrossNavigationService navigationService)
            : base(navigationService, logger)
        {
        }
    }
}
