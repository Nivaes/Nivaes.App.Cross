namespace Nivaes.App.Cross
{
    using MvvmCross.IoC;
    using Nivaes.App.Cross.Visibility;

    [Obsolete("", true)]
    public abstract class CrossBasePlugin 
        : IMvxPlugin
    {
        public virtual void Load(IMvxIoCProvider provider)
        {
            if (provider.TryResolve(out ICrossValueConverterRegistry? registry))
                RegisterValueConverters(registry);
        }

        private static void RegisterValueConverters(ICrossValueConverterRegistry registry)
        {
            registry.AddOrOverwrite("Visibility", new CrossVisibilityValueConverter());
            registry.AddOrOverwrite("InvertedVisibility", new CrossInvertedVisibilityValueConverter());
        }
    }
}