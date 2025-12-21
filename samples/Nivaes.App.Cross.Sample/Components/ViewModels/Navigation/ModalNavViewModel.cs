namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class ModalNavViewModel : MvxNavigationViewModel
    {
        public ModalNavViewModel(ILoggerFactory logFactory, IMvxNavigationService navigationService) : base(logFactory, navigationService)
        {
            CloseCommand = new CrossAsyncCommand(() => NavigationService.Close(this));

            ShowChildCommand = new CrossAsyncCommand(() => NavigationService.Navigate<ChildViewModel>());

            ShowNestedModalCommand = new CrossAsyncCommand(() => NavigationService.Navigate<NestedModalViewModel>());
        }

        public ICrossAsyncCommand CloseCommand { get; private set; }

        public ICrossAsyncCommand ShowChildCommand { get; private set; }

        public ICrossAsyncCommand ShowNestedModalCommand { get; private set; }
    }
}
