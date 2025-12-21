namespace Playground.Core.ViewModels.Navigation
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.Navigation;
    using Nivaes.App.Cross;

    public class FragmentCloseViewModel : BaseViewModel
    {
        private static int _counter = 0;

        public FragmentCloseViewModel(ILoggerFactory loggerFactory, IMvxNavigationService navigationService)
            : base(loggerFactory, navigationService)
        {
            ForwardCommand = new CrossAsyncCommand(() => NavigationService.Navigate<FragmentCloseViewModel>());
            CloseCommand = new CrossAsyncCommand(() => NavigationService.Close(this));

            Description = $"View number {_counter++}";
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public ICrossAsyncCommand ForwardCommand { get; }
        public ICrossAsyncCommand CloseCommand { get; }
    }
}
