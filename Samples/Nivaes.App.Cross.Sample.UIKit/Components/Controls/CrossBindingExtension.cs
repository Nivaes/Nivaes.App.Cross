using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross.Sample.UIKitLib
{
    public static class CrossBindingExtension
    {
        extension(IServiceProvider service)
        {
            // Registrar con roslyn.

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
