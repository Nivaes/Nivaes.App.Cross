namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.Presenters.Hints;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class Tab3ViewModel : MvxNavigationViewModel
    {
        public Tab3ViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            ShowRootViewModelCommand = new CrossAsyncCommand(() => NavigationService.Navigate<RootViewModel>());

            CloseViewModelCommand = new CrossAsyncCommand(() => NavigationService.Close(this));

            ShowPageOneCommand = new CrossCommand(() => NavigationService.ChangePresentation(new MvxPagePresentationHint(typeof(Tab1ViewModel))));
        }

        public ICrossAsyncCommand ShowRootViewModelCommand { get; }

        public ICrossAsyncCommand CloseViewModelCommand { get; }

        public ICrossCommand ShowPageOneCommand { get; }
    }
}
