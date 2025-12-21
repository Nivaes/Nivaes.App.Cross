namespace Playground.Core.ViewModels
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class SplitRootViewModel : MvxNavigationViewModel
    {
        public SplitRootViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            ShowInitialMenuCommand = new CrossAsyncCommand(ShowInitialViewModel);
            ShowDetailCommand = new CrossAsyncCommand(ShowDetailViewModel);
        }

        public ICrossAsyncCommand ShowInitialMenuCommand { get; }

        public ICrossAsyncCommand ShowDetailCommand { get; }

        public override void ViewAppeared()
        {
            MvxNotifyTask.Create(async () =>
            {
                await ShowInitialViewModel();
                await ShowDetailViewModel();
            });
        }

        private Task ShowInitialViewModel()
        {
            return NavigationService.Navigate<SplitMasterViewModel>();
        }

        private Task ShowDetailViewModel()
        {
            return NavigationService.Navigate<SplitDetailViewModel>();
        }
    }
}
