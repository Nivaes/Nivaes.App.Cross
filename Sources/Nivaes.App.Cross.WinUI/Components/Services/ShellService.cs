using Microsoft.Extensions.DependencyInjection;
using Nivaes.App.Cross;

namespace Nivaes.App.Cross.WinUI
{
    public sealed class ShellService
         : IShellService
    {
        private readonly CrossNavigationService _navigationService;

        /// <summary>Initializes a new instance of the <see cref="ClientIdentifyService"/> class.</summary>
        public ShellService(CrossNavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        async ValueTask IShellService.InitializeShell()
        {
            await _navigationService.Navigate<ShellViewModel>();

            await Task.Delay(100);
        }

        ValueTask IShellService.ShowMenu()
        {
            return new ValueTask();
        }
    }
}
