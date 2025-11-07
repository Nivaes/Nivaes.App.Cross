namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    public static class CrossRegistryFillerExtensions
    {
        [Obsolete("No compatible con AoT", true)]
        [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
        public static void Fill<T>(
            this ICrossNamedInstanceRegistry<T> registry, IEnumerable<Assembly> assemblies, IEnumerable<Type> types)
            where T : notnull
        {
            throw new NotImplementedException();
            //var filler = Cross.IoCProvider.Resolve<ICrossNamedInstanceRegistryFiller<T>>();
            //registry.Fill(filler, assemblies);
            //registry.Fill(filler, types);
        }

        [Obsolete("No compatible con AoT", true)]
        [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
        public static void Fill<T>(this ICrossNamedInstanceRegistry<T> registry, IEnumerable<Assembly> assemblies)
            where T : notnull
        {
            throw new NotImplementedException();
            //if (assemblies == null)
            //    return;

            //var filler = Cross.IoCProvider.Resolve<ICrossNamedInstanceRegistryFiller<T>>();
            //registry.Fill(filler, assemblies);
        }

        [Obsolete("No compatible con AoT", true)]
        [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
        public static void Fill<T>(
            this ICrossNamedInstanceRegistry<T> registry, ICrossNamedInstanceRegistryFiller<T> filler,
            IEnumerable<Assembly> assemblies)
            where T : notnull
        {
            if (assemblies == null)
                return;

            foreach (var assembly in assemblies)
            {
                registry.Fill(filler, assembly);
            }
        }

        [Obsolete("No compatible con AoT", true)]
        [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
        public static void Fill<T>(this ICrossNamedInstanceRegistry<T> registry, Assembly assembly)
            where T : notnull
        {
            throw new NotImplementedException();

            //var filler = Cross.IoCProvider.Resolve<ICrossNamedInstanceRegistryFiller<T>>();
            //registry.Fill(filler, assembly);
        }

        [Obsolete("No compatible con AoT", true)]
        [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
        public static void Fill<T>(this ICrossNamedInstanceRegistry<T> registry, ICrossNamedInstanceRegistryFiller<T> filler,
                                Assembly assembly)
            where T : notnull
        {
            filler.FillFrom(registry, assembly);
        }

        [Obsolete("No compatible con AoT", true)]
        [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
        public static void Fill<T>(this ICrossNamedInstanceRegistry<T> registry, IEnumerable<Type> types)
            where T : notnull
        {
            throw new NotImplementedException();

            //if (types == null)
            //    return;

            //var filler = Cross.IoCProvider.Resolve<ICrossNamedInstanceRegistryFiller<T>>();
            //registry.Fill(filler, types);
        }

        [Obsolete("No compatible con AoT", true)]
        [RequiresUnreferencedCode("This method uses reflection to check for creatable types, which may not be preserved by trimming")]
        public static void Fill<T>(
            this ICrossNamedInstanceRegistry<T> registry,
            ICrossNamedInstanceRegistryFiller<T> filler,
            IEnumerable<Type> types)
        {
            if (types == null)
                return;

            foreach (var type in types)
            {
                registry.Fill(filler, type);
            }
        }

        [Obsolete("No compatible con AoT", true)]
        public static void Fill<T>(
            this ICrossNamedInstanceRegistry<T> registry, ICrossNamedInstanceRegistryFiller<T> filler,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicFields)] Type type)
            where T : notnull
        {
            filler.FillFrom(registry, type);
        }

        [Obsolete("No compatible con AoT", true)]
        public static void Fill<T>(
            this ICrossNamedInstanceRegistry<T> registry,
            [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor | DynamicallyAccessedMemberTypes.PublicFields)] Type type)
            where T : notnull
        {
            throw new NotImplementedException();

            //var filler = Cross.IoCProvider.Resolve<ICrossNamedInstanceRegistryFiller<T>>();
            //registry.Fill(filler, type);
        }
    }
}
