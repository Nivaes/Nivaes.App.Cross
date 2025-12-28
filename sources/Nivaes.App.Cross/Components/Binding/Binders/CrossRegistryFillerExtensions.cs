namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;
    using Nivaes.IoC;

    public static class CrossRegistryFillerExtensions
    {
        extension<T>(ICrossNamedInstanceRegistry<T> registry) where T : notnull
        {
            [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
            public void Fill(IEnumerable<Assembly> assemblies, IEnumerable<Type> types)
            {
                var filler = Mvx.IoCProvider.Resolve<IMvxNamedInstanceRegistryFiller<T>>();
                registry.Fill(filler, assemblies);
                registry.Fill(filler, types);
            }

            [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
            public void Fill(IEnumerable<Assembly> assemblies)
            {
                if (assemblies == null)
                    return;

                var filler = Mvx.IoCProvider.Resolve<IMvxNamedInstanceRegistryFiller<T>>();
                registry.Fill(filler, assemblies);
            }

            [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
            public void Fill(IMvxNamedInstanceRegistryFiller<T> filler, IEnumerable<Assembly> assemblies)
            {
                if (assemblies == null)
                    return;

                foreach (var assembly in assemblies)
                {
                    registry.Fill(filler, assembly);
                }
            }

            [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
            public void Fill(Assembly assembly)
            {
                var filler = Mvx.IoCProvider.Resolve<IMvxNamedInstanceRegistryFiller<T>>();
                registry.Fill(filler, assembly);
            }

            [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
            public void Fill(IMvxNamedInstanceRegistryFiller<T> filler,
                                    Assembly assembly)
            {
                filler.FillFrom(registry, assembly);
            }

            [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
            public void Fill(IEnumerable<Type> types)
            {
                if (types == null)
                    return;

                var filler = Mvx.IoCProvider.Resolve<IMvxNamedInstanceRegistryFiller<T>>();
                registry.Fill(filler, types);
            }

            public void Fill(IMvxNamedInstanceRegistryFiller<T> filler,
                [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicFields)] Type type)
            {
                filler.FillFrom(registry, type);
            }

            public void Fill(
                [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicFields)] Type type)
            {
                var filler = Mvx.IoCProvider.Resolve<IMvxNamedInstanceRegistryFiller<T>>();
                registry.Fill(filler, type);
            }
        }

        extension<T>(ICrossNamedInstanceRegistry<T> registry)
        {
            [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
            public void Fill(
            IMvxNamedInstanceRegistryFiller<T> filler,
            IEnumerable<Type> types)
            {
                if (types == null)
                    return;

                foreach (var type in types)
                {
                    registry.Fill(filler, type);
                }
            }
        }
    }
}
