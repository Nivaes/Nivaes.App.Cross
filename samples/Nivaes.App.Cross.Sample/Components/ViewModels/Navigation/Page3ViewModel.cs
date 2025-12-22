namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class Page3ViewModel 
        : CrossNavigationViewModel
    {
        public Page3ViewModel(ILoggerFactory logProvider, ICrossNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }
    }
}
