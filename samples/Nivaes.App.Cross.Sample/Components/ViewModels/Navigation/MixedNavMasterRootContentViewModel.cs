namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;
    using Playground.Core.Models;

    public class MixedNavMasterRootContentViewModel : CrossNavigationViewModel
    {
        public MixedNavMasterRootContentViewModel(ILoggerFactory logProvider, ICrossNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            ShowModalCommand = new CrossAsyncCommand(() => NavigationService.Navigate<ModalNavViewModel>());
            ShowChildCommand = new CrossAsyncCommand(() => NavigationService.Navigate<ChildViewModel, SampleModel>(new SampleModel("Hey", 1.23m)));
        }

        public ICrossAsyncCommand ShowModalCommand { get; }
        public ICrossAsyncCommand ShowChildCommand { get; }
    }
}
