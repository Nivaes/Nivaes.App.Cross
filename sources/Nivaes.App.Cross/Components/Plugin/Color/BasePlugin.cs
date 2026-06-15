using MvvmCross.IoC;

namespace Nivaes.App.Cross;

[Obsolete("", true)]
public abstract class BasePlugin : IMvxPlugin
{
    public virtual void Load(IMvxIoCProvider provider)
    {
        if (provider.TryResolve<ICrossValueConverterRegistry>(out var registry) && registry != null)
        {
            registry.AddOrOverwrite("ARGB", new CrossARGBValueConverter());
            registry.AddOrOverwrite("NativeColor", new CrossNativeColorValueConverter());
            registry.AddOrOverwrite("RGBA", new CrossRGBAValueConverter());
            registry.AddOrOverwrite("RGB", new CrossRGBValueConverter());
            registry.AddOrOverwrite("RGBIntColor", new CrossRGBIntColorValueConverter());
        }
    }
}
