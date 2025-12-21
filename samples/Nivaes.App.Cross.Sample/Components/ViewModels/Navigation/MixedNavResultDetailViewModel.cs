namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class MixedNavResultDetailViewModel : MvxNavigationViewModel
    {
        public MixedNavResultDetailViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            CloseViewModelCommand = new CrossAsyncCommand(() => NavigationService.Close(this));
        }

        public ICrossAsyncCommand CloseViewModelCommand { get; }
    }
}
