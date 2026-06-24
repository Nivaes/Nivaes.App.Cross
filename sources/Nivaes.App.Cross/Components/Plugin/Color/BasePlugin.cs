using MvvmCross.IoC;

namespace Nivaes.App.Cross;

[Obsolete("", true)]
public abstract class BasePlugin : IMvxPlugin
{
    public virtual void Load(IMvxIoCProvider provider)
    {
        if (provider.TryResolve<ICrossValueConverterRegistry>(out var registry) && registry != null)
        {
            registry.AddOrOverwrite("ARGB", new CrossARGBValueConverter(null, null));
            registry.AddOrOverwrite("NativeColor", new CrossNativeColorValueConverter(null, null));
            registry.AddOrOverwrite("RGBA", new CrossRGBAValueConverter(null, null));
            registry.AddOrOverwrite("RGB", new CrossRGBValueConverter(null, null));
            registry.AddOrOverwrite("RGBIntColor", new CrossRGBIntColorValueConverter(null, null));
        }
    }
}
