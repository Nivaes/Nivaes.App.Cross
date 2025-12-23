namespace Playground.Core.ViewModels.Bindings
{
    using Microsoft.Extensions.Logging;
    using Nivaes.App.Cross;

    public class CustomBindingViewModel
        : CrossNavigationViewModel
    {
        private ICrossAsyncCommand _closeCommand;

        private int _counter = 2;

        private DateTime _date = DateTime.Now;

        private string _hello = "Hello MvvmCross";

        public CustomBindingViewModel(ILoggerFactory logFactory, ICrossNavigationService navigationService)
            : base(logFactory, navigationService)
        {
        }

        public string Hello
        {
            get => _hello;
            set => SetProperty(ref _hello, value);
        }

        public ICrossAsyncCommand CloseCommand =>
            _closeCommand ??= new CrossAsyncCommand(() => NavigationService.Close(this));

        public int Counter
        {
            get => _counter;
            set => SetProperty(ref _counter, value);
        }

        public DateTime Date
        {
            get => _date;
            set => SetProperty(ref _date, value);
        }
    }
}
