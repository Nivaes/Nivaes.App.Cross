namespace Playground.Core.ViewModels
{
    using System.Threading.Tasks;
    using MvvmCross;
    using MvvmCross.Navigation;
    using Nivaes.App.Cross;

    public class NavigationCloseViewModel 
        : CrossViewModel
    {
        private readonly IMvxNavigationService _mvxNavigationService;

        public NavigationCloseViewModel(IMvxNavigationService mvxNavigationService)
        {
            _mvxNavigationService = mvxNavigationService;
        }

        public ICrossAsyncCommand OpenChildThenCloseThisCommand => new CrossAsyncCommand(CloseThisAndOpenChildAsync);

        public ICrossAsyncCommand TryToCloseNewViewModelCommand => new CrossAsyncCommand(TryToCloseNewViewModelAsync);

        private async Task CloseThisAndOpenChildAsync()
        {
            await _mvxNavigationService.Navigate<SecondChildViewModel>();
            await _mvxNavigationService.Close(this);
        }

        private Task TryToCloseNewViewModelAsync()
        {
            return _mvxNavigationService.Close(Mvx.IoCProvider.Resolve<SecondChildViewModel>());
        }
    }
}
