namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.Presenters.Hints;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class NestedChildViewModel : MvxNavigationViewModel
    {
        public NestedChildViewModel(ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            CloseCommand = new CrossAsyncCommand(() => NavigationService.Close(this));
            PopToChildCommand = new CrossAsyncCommand(() => NavigationService.ChangePresentation(new MvxPopPresentationHint(typeof(ChildViewModel))));
            PopToRootCommand = new CrossAsyncCommand(() => NavigationService.ChangePresentation(new MvxPopToRootPresentationHint()));
            RemoveCommand = new CrossAsyncCommand(() => NavigationService.ChangePresentation(new MvxRemovePresentationHint(typeof(SecondChildViewModel))));
        }

        public ICrossAsyncCommand CloseCommand { get; }

        public ICrossAsyncCommand PopToChildCommand { get; }

        public ICrossAsyncCommand PopToRootCommand { get; }

        public ICrossAsyncCommand RemoveCommand { get; }
    }
}
