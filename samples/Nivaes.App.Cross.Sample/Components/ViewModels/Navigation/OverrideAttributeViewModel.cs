namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class OverrideAttributeViewModel : MvxNavigationViewModel
    {
        public OverrideAttributeViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            CloseCommand = new CrossAsyncCommand(() => NavigationService.Close(this));

            ShowTabsCommand = new CrossAsyncCommand(() => NavigationService.Navigate<TabsRootViewModel>());

            ShowSecondChildCommand = new CrossAsyncCommand(() => NavigationService.Navigate<SecondChildViewModel>());
        }

        public ICrossAsyncCommand ShowTabsCommand { get; }

        public ICrossAsyncCommand CloseCommand { get; }

        public ICrossAsyncCommand ShowSecondChildCommand { get; }
    }
}
