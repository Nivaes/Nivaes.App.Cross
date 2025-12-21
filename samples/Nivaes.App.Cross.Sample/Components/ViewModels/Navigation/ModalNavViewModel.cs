namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class ModalNavViewModel
        : CrossNavigationViewModel
    {
        public ModalNavViewModel(ILoggerFactory logFactory, ICrossNavigationService navigationService) : base(logFactory, navigationService)
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
