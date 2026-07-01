using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.Metrics;

namespace Nivaes.App.Cross.Hosting;

internal class CrossMetricsBuilder(IServiceCollection services) : IMetricsBuilder
{
    readonly IServiceCollection _services = services;

    public IServiceCollection Services => _services;
}
