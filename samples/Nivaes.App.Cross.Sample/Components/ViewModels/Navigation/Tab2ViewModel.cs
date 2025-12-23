namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    public class Tab2ViewModel 
        : CrossNavigationViewModel
    {
        public Tab2ViewModel(ILoggerFactory logProvider, ICrossNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            ShowRootViewModelCommand = new CrossAsyncCommand(() => NavigationService.Navigate<RootViewModel>());

            CloseViewModelCommand = new CrossAsyncCommand(() => NavigationService.Close(this));
        }

        public ICrossAsyncCommand ShowRootViewModelCommand { get; }

        public ICrossAsyncCommand CloseViewModelCommand { get; }
    }
}
