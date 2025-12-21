namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class SheetViewModel : MvxNavigationViewModel
    {
        public SheetViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            CloseCommand = new CrossAsyncCommand(CloseSheet);
        }

        public ICrossAsyncCommand CloseCommand { get; }

        private Task CloseSheet()
        {
            return NavigationService.Close(this);
        }
    }
}
