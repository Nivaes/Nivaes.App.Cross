namespace Nivaes.App.Cross
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Reflection;

    [Obsolete("Quitar IoC de Cross")]
    public static class CrossSetupExtensions
    {
        public static void RegisterSetupType<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] TMvxSetup>(this object platformApplication, params Assembly[]? assemblies)
            where TMvxSetup : CrossSetup, new()
        {
            if (platformApplication == null)
                throw new ArgumentNullException(nameof(platformApplication));

            CrossSetup.RegisterSetupType<TMvxSetup>(
                new[] { platformApplication.GetType().Assembly }.Union(assemblies ?? []).ToArray());
        }

        [RequiresUnreferencedCode("This method uses reflection to find types, which may not be preserved in trimmed applications")]
        public static TSetup? CreateSetup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TSetup>(Assembly assembly, params object[] parameters)
            where TSetup : CrossSetup
        {
            var setupType = FindSetupType<TSetup>(assembly);
            if (setupType == null)
            {
                throw new CrossException("Could not find a Setup class for application");
            }

            try
            {
                return (TSetup?)Activator.CreateInstance(setupType, parameters);
            }
            catch (Exception exception)
            {
                throw exception.Wrap("Failed to create instance of {0}", setupType.FullName);
            }
        }

        [RequiresUnreferencedCode("This method uses reflection to find types, which may not be preserved in trimmed applications")]
        public static TSetup? CreateSetup<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TSetup>() 
            where TSetup : CrossSetup
        {
            var setupType = FindSetupType<TSetup>();
            if (setupType == null)
            {
                throw new CrossException("Could not find a Setup class for application");
            }

            try
            {
                return (TSetup?)Activator.CreateInstance(setupType);
            }
            catch (Exception exception)
            {
                throw exception.Wrap("Failed to create instance of {0}", setupType.FullName);
            }
        }

        [RequiresUnreferencedCode("This method uses reflection to find types, which may not be preserved in trimmed applications")]
        public static Type? FindSetupType<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)] TSetup>(Assembly assembly)
        {
            throw new NotImplementedException();
            //var query = from type in assembly.ExceptionSafeGetTypes()
            //            where type.Name == "Setup"
            //            where typeof(TSetup).IsAssignableFrom(type)
            //            select type;

            //return query.FirstOrDefault();
        }

        [RequiresUnreferencedCode("This method uses reflection to find types, which may not be preserved in trimmed applications")]
        public static Type? FindSetupType<TSetup>()
        {
            throw new NotImplementedException();

            //var query = from assembly in AppDomain.CurrentDomain.GetAssemblies()
            //            from type in assembly.ExceptionSafeGetTypes()
            //            where type.Name == "Setup"
            //            where typeof(TSetup).IsAssignableFrom(type)
            //            select type;

            //return query.FirstOrDefault();
        }
    }
}