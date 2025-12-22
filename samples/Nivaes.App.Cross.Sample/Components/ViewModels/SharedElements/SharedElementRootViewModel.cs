namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    public class SharedElementRootViewModel 
        : BaseViewModel
    {
        public SharedElementRootViewModel(ILoggerFactory logProvider, ICrossNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }
    }
}
