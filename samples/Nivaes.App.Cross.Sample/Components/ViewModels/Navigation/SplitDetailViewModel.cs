namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class SplitDetailViewModel : MvxNavigationViewModel
    {
        public SplitDetailViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            ShowChildCommand = new CrossAsyncCommand(() => NavigationService.Navigate<SplitDetailNavViewModel>());
            ShowTabsCommand = new CrossAsyncCommand(() => NavigationService.Navigate<TabsRootBViewModel>());
            ShowTabbedChildCommand = new CrossAsyncCommand(() => NavigationService.Navigate<TabsRootViewModel>());
        }

        public ICrossAsyncCommand ShowChildCommand { get; }
        public ICrossAsyncCommand ShowTabsCommand { get; }
        public ICrossAsyncCommand ShowTabbedChildCommand { get; }

        public string ContentText => "Text for the Content Area";
    }
}
