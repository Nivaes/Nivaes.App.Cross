namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class BaseViewModel : CrossNavigationViewModel
    {
        public BaseViewModel(ILoggerFactory loggerFactory, ICrossNavigationService navigationService)
            : base(loggerFactory, navigationService)
        {
        }
    }
}
