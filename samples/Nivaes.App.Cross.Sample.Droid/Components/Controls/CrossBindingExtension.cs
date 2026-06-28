using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross.Sample.Droid
{
    public static class CrossBindingExtension
    {
        extension(IServiceProvider service)
        {
            public IServiceProvider TargetBindingFactoryRegistry()
            {
                // Registrar con roslyn.

                var registry = service.GetRequiredService<ICrossTargetBindingFactoryRegistry>();

                registry.RegisterCustomBindingFactory<BinaryEdit>(
                   "MyCount",
                   (arg) => new BinaryEditTargetBinding(arg));

                return service;
            }
        }
    }
}
