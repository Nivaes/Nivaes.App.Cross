namespace Playground.Core.ViewModels
{
    using System.Threading.Tasks;
    using MvvmCross.Commands;
    using MvvmCross.Navigation;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class NativeViewModel 
        : CrossViewModel
    {
        private static int _counter = 0;

        public NativeViewModel(IMvxNavigationService navigationService)
        {
            ForwardCommand = new MvxAsyncCommand(() => navigationService.Navigate<NativeViewModel>());
            CloseCommand = new MvxAsyncCommand(() => navigationService.Close(this));

            Description = $"View number {_counter++}";
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public IMvxAsyncCommand ForwardCommand { get; }
        public IMvxAsyncCommand CloseCommand { get; }

    }
}
