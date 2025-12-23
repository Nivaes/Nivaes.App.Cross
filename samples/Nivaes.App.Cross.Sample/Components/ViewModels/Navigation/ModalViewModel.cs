namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    public class ModalViewModel 
        : CrossNavigationViewModel
    {
        public ModalViewModel(ILoggerFactory logProvider, ICrossNavigationService navigationService) : base(logProvider, navigationService)
        {
            ShowTabsCommand = new CrossAsyncCommand(() => NavigationService.Navigate<TabsRootViewModel>());

            CloseCommand = new CrossAsyncCommand(() => NavigationService.Close(this));

            ShowNestedModalCommand = new CrossAsyncCommand(() => NavigationService.Navigate<NestedModalViewModel>());
        }

        public ICrossAsyncCommand ShowTabsCommand { get; }

        public ICrossAsyncCommand CloseCommand { get; }

        public ICrossAsyncCommand ShowNestedModalCommand { get; }
    }
}
