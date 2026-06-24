using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross.Sample.Droid
{
    public static class CrossBindingExtension
    {
        extension(IServiceProvider service)
        {
            public IServiceProvider TargetBindingFactoryRegistry()
            {
                var registry = service.GetRequiredService<ICrossTargetBindingFactoryRegistry>();

                registry.RegisterCustomBindingFactory<BinaryEdit>(
                   "MyCount",
                   (arg) => new BinaryEditTargetBinding(arg));

                return service;
            }
        }
    }
}
