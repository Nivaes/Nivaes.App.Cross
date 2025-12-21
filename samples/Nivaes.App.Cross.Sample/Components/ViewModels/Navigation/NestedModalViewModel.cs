namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class NestedModalViewModel : MvxNavigationViewModel
    {
        public NestedModalViewModel(ILoggerFactory logFactory, ICrossNavigationService navigationService)
            : base(logFactory, navigationService)
        {
            CloseCommand = new CrossAsyncCommand(() => NavigationService.Close(this));

            ShowTabsCommand = new CrossAsyncCommand(() => NavigationService.Navigate<TabsRootViewModel>());
        }

        public ICrossAsyncCommand ShowTabsCommand { get; }

        public ICrossAsyncCommand CloseCommand { get; }
    }
}
