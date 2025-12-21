namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class SplitMasterViewModel : MvxNavigationViewModel
    {
        public SplitMasterViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            OpenDetailCommand = new CrossAsyncCommand(() => NavigationService.Navigate<SplitDetailViewModel>());

            OpenDetailNavCommand = new CrossAsyncCommand(() => NavigationService.Navigate<SplitDetailNavViewModel>());

            ShowRootViewModel = new CrossAsyncCommand(() => NavigationService.Navigate<RootViewModel>());
        }

        public string PaneText => "Text for the Master Pane";

        public ICrossAsyncCommand OpenDetailCommand { get; }

        public ICrossAsyncCommand OpenDetailNavCommand { get; }

        public ICrossAsyncCommand ShowRootViewModel { get; }
    }
}
