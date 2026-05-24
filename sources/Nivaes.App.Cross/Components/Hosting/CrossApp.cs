using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross.Hosting
{
    public sealed class CrossApp : ICrossApp, IDisposable, IAsyncDisposable
    {
        private readonly IServiceProvider _services;

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

        private void DisposeConfiguration()
        {
            // Explicitly dispose the Configuration, since it is added as a singleton object that the ServiceProvider
            // won't dispose.
            (Configuration as IDisposable)?.Dispose();
        }
    }
}
