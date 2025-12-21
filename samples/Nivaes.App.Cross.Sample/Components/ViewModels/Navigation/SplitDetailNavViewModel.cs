namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class SplitDetailNavViewModel : MvxNavigationViewModel
    {
        public SplitDetailNavViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            MainMenuCommand = new CrossAsyncCommand(() => NavigationService.Navigate<MixedNavFirstViewModel>());
            CloseCommand = new CrossAsyncCommand(() => NavigationService.Close(this));
        }

        public ICrossAsyncCommand MainMenuCommand { get; }
        public ICrossAsyncCommand CloseCommand { get; }

    }
}
