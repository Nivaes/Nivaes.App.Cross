namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    public class SharedElementSecondChildViewModel 
        : BaseViewModel
    {
        public SharedElementSecondChildViewModel(ILoggerFactory logProvider, ICrossNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }
    }
}
