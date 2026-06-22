namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    public class PagesRootViewModel 
        : CrossNavigationViewModel
    {
        public PagesRootViewModel(ILogger<PagesRootViewModel> logger, ICrossNavigationService navigationService)
            : base(navigationService, logger)
        {
            ShowInitialViewModelsCommand = new CrossAsyncCommand(ShowInitialViewModels);
        }

        public ICrossAsyncCommand ShowInitialViewModelsCommand { get; }

        private Task ShowInitialViewModels()
        {
            var tasks = new List<Task>();
            tasks.Add(NavigationService.Navigate<Page1ViewModel>());
            tasks.Add(NavigationService.Navigate<Page2ViewModel>());
            tasks.Add(NavigationService.Navigate<Page3ViewModel>());
            return Task.WhenAll(tasks);
        }
    }
}
