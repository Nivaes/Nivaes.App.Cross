namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    public class Page2ViewModel 
        : CrossNavigationViewModel
    {
        public Page2ViewModel(ILogger<Page2ViewModel> loggger, ICrossNavigationService navigationService)
            : base(navigationService, loggger)
        {
        }
    }
}
