namespace Playground.Core.ViewModels
{
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.Presenters.Hints;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class Tab1ViewModel : MvxNavigationViewModel<string>
    {
        public Tab1ViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            OpenChildCommand = new CrossAsyncCommand(() => NavigationService.Navigate<ChildViewModel>());

            OpenModalCommand = new CrossAsyncCommand(() => NavigationService.Navigate<ModalViewModel>());

            OpenNavModalCommand = new CrossAsyncCommand(() => NavigationService.Navigate<ModalNavViewModel>());

            CloseCommand = new CrossAsyncCommand(() => NavigationService.Close(this));

            OpenTab2Command = new CrossAsyncCommand(() => NavigationService.ChangePresentation(new MvxPagePresentationHint(typeof(Tab2ViewModel))));
        }

        public override Task Initialize()
        {
            return Task.Delay(3000);
        }

        string para;
        public override void Prepare(string parameter)
        {
            para = parameter;
        }

        public ICrossAsyncCommand OpenChildCommand { get; }

        public ICrossAsyncCommand OpenModalCommand { get; }

        public ICrossAsyncCommand OpenNavModalCommand { get; }

        public ICrossAsyncCommand OpenTab2Command { get; }

        public ICrossAsyncCommand CloseCommand { get; }
    }
}
