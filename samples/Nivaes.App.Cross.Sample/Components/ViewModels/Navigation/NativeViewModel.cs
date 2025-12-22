namespace Playground.Core.ViewModels
{
    using Nivaes.App.Cross;

    public class NativeViewModel 
        : CrossViewModel
    {
        private static int _counter = 0;

        public NativeViewModel(ICrossNavigationService navigationService)
        {
            ForwardCommand = new CrossAsyncCommand(() => navigationService.Navigate<NativeViewModel>());
            CloseCommand = new CrossAsyncCommand(() => navigationService.Close(this));

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
