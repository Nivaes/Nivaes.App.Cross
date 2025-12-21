namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class BaseViewModel : MvxNavigationViewModel
    {
        public BaseViewModel(ILoggerFactory loggerFactory, ICrossNavigationService navigationService)
            : base(loggerFactory, navigationService)
        {
        }
    }
}
