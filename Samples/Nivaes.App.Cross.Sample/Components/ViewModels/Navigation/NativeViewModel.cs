using Microsoft.Extensions.Logging;
using Nivaes.App.Cross;

namespace Playground.Core.ViewModels
{
    public class NativeViewModel
        : CrossViewModel
    {
        private static int _counter = 0;

        public NativeViewModel(ICrossNavigationService navigationService, ILogger<NativeViewModel> logger)
            : base(logger)
        {
            ForwardCommand = new CrossAsyncCommand(() => navigationService.Navigate<NativeViewModel>());
            CloseCommand = new CrossAsyncCommand(() => navigationService.Close(this));

            Description = $"View number {_counter++}";
        }

        private string? _description;

        public string? Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        public ICrossAsyncCommand ForwardCommand { get; }
        public ICrossAsyncCommand CloseCommand { get; }

    }
}
