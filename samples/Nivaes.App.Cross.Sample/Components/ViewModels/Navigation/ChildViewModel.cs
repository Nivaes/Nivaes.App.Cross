namespace Playground.Core.ViewModels
{
    using Microsoft.Extensions.Logging;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;
    using Playground.Core.Models;

    public class ChildViewModel : MvxNavigationViewModel<SampleModel>
    {
        public string BrokenTextValue { get => _brokenTextValue; set => SetProperty(ref _brokenTextValue, value); }
        public string AnotherBrokenTextValue { get => _anotherBrokenTextValue; set => SetProperty(ref _anotherBrokenTextValue, value); }

        private string _brokenTextValue;
        private string _anotherBrokenTextValue;

        public ChildViewModel(
            ILoggerFactory logProvider, IMvxNavigationService navigationService)
            : base(logProvider, navigationService)
        {
            CloseCommand = new CrossAsyncCommand(DoCloseCommand);

            ShowSecondChildCommand = new CrossAsyncCommand(() => NavigationService.Navigate<SecondChildViewModel>());

            ShowRootCommand = new CrossAsyncCommand(() => NavigationService.Navigate<RootViewModel>());

            PropertyChanged += ChildViewModel_PropertyChanged;
        }

        private Task DoCloseCommand()
        {
            return NavigationService.Close(this);
        }

        private void ChildViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Demonstrates that exceptions can be raised on property changed but are swallowed by default to 
            // protect the app from crashing
            if (e.PropertyName == nameof(BrokenTextValue))
                throw new System.NotImplementedException();
        }

        public override async System.Threading.Tasks.Task Initialize()
        {
            await base.Initialize();

            await Task.Delay(8500);
        }

        public void Init()
        {
            // Method intentionally left empty.
        }

        public ICrossAsyncCommand CloseCommand { get; private set; }

        public ICrossAsyncCommand ShowSecondChildCommand { get; private set; }

        public ICrossAsyncCommand ShowRootCommand { get; private set; }

        public override void Prepare(SampleModel parameter)
        {
        }

        public override void ViewAppeared()
        {
            base.ViewAppeared();

            Task.Run(async () =>
            {
                await Task.Delay(1000);
                BrokenTextValue = "This will throw exception in UI layer";
                AnotherBrokenTextValue = "This will throw exception in page";
            });
        }
    }
}
