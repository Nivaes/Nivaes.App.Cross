using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Nivaes.App.Cross.Hosting
{
    public sealed class CrossApp : ICrossApp, IHost, IDisposable, IAsyncDisposable
    {
        private readonly IServiceProvider _services;

        private List<Task>? _backgroundServiceTasks;

        internal CrossApp(IServiceProvider services)
        {
            _services = services;
        }

        public IServiceProvider Services => _services;

        public IConfiguration Configuration => _services.GetRequiredService<IConfiguration>();

        public static CrossAppBuilder CreateBuilder(bool useDefaults = true) => new(useDefaults);

        /// <inheritdoc />
        public void Dispose()
        {
            DisposeConfiguration();

            (_services as IDisposable)?.Dispose();
        }

        /// <inheritdoc />
        public async ValueTask DisposeAsync()
        {
            DisposeConfiguration();

            if (_services is IAsyncDisposable asyncDisposable)
            {
                // Fire and forget because this is called from a sync context
                await asyncDisposable.DisposeAsync();
            }
            else
            {
                (_services as IDisposable)?.Dispose();
            }
        }

        public async Task StartAsync(CancellationToken cancellationToken = default)
        {
            var services = _services.GetRequiredService<IEnumerable<IHostedService>>();
            foreach (var service in services)
            {
                if (service is BackgroundService backgroundService)
                {
                    await backgroundService.StartAsync(CancellationToken.None);
                }
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken = default)
        {
            var services = _services.GetRequiredService<IEnumerable<IHostedService>>();
            foreach (var service in services)
            {
                if (service is BackgroundService backgroundService)
                {
                    await backgroundService.StopAsync(CancellationToken.None);
                }
            }
        }

        private void DisposeConfiguration()
        {
            (Configuration as IDisposable)?.Dispose();
        }
    }
}
