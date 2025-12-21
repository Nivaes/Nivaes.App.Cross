namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class PagesRootViewModel 
        : MvxNavigationViewModel
    {
        public PagesRootViewModel(ILoggerFactory logProvider, ICrossNavigationService navigationService)
            : base(logProvider, navigationService)
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
