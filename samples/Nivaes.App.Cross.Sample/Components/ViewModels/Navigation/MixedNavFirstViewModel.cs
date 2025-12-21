namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class MixedNavFirstViewModel : MvxNavigationViewModel
    {
        public MixedNavFirstViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
        }

        public ICrossAsyncCommand LoginCommand => new CrossAsyncCommand(GotoMasterDetailPage, CanLogin);

        private bool CanLogin()
        {
            return true;
        }

        private async Task GotoMasterDetailPage()
        {
            await NavigationService.Navigate<MixedNavMasterDetailViewModel>();
            await NavigationService.Navigate<MixedNavMasterRootContentViewModel>();
        }
    }
}
