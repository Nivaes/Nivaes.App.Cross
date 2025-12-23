namespace Nivaes.App.Cross
{
    using MvvmCross.IoC;
    using Nivaes.App.Cross.Visibility;

    public abstract class BasePlugin 
        : IMvxPlugin
    {
        public virtual void Load(IMvxIoCProvider provider)
        {
            if (provider.TryResolve(out IMvxValueConverterRegistry registry))
                RegisterValueConverters(registry);
        }

        private static void RegisterValueConverters(IMvxValueConverterRegistry registry)
        {
            registry.AddOrOverwrite("Visibility", new MvxVisibilityValueConverter());
            registry.AddOrOverwrite("InvertedVisibility", new MvxInvertedVisibilityValueConverter());
        }
    }
}