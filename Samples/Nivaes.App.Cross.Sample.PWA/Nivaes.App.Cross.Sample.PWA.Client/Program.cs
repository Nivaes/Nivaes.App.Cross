using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace Nivaes.App.Cross.Sample.PWA.Client;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = WebAssemblyHostBuilder.CreateDefault(args);

        await builder.Build().RunAsync();
    }
}
