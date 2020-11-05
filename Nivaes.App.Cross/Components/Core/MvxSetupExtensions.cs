namespace MvvmCross.Core
{
    using System;
    using System.Linq;
    using System.Reflection;
    using MvvmCross.Exceptions;
    using MvvmCross.IoC;

    public static class MvxSetupExtensions
    {
        public static void RegisterSetupType<TMvxSetup>(this object platformApplication,  params Assembly[] assemblies) where TMvxSetup : MvxSetup, new()
        {
            if (platformApplication == null) throw new ArgumentNullException(nameof(platformApplication));

            MvxSetup.RegisterSetupType<TMvxSetup>(new[] { platformApplication.GetType().Assembly }.Union(assemblies ?? new Assembly[] { }).ToArray());
        }

        public static TSetup CreateSetup<TSetup>(Assembly assembly, params object[] parameters) where TSetup : MvxSetup
        {
            var setupType = FindSetupType<TSetup>(assembly);
            if (setupType == null)
            {
                throw new MvxException("Could not find a Setup class for application");
            }

            try
            {
                return (TSetup)Activator.CreateInstance(setupType, parameters);
            }
            catch (Exception ex)
            {
                throw new MvxException($"Failed to create instance of {setupType.FullName}", ex);
            }
        }

        public static TSetup CreateSetup<TSetup>() where TSetup : MvxSetup
        {
            var setupType = FindSetupType<TSetup>();
            if (setupType == null)
            {
                throw new MvxException("Could not find a Setup class for application");
            }

            try
            {
                return (TSetup)Activator.CreateInstance(setupType);
            }
            catch (Exception ex)
            {
                throw new MvxException($"Failed to create instance of {setupType.FullName}", ex);
            }
        }

        public static Type FindSetupType<TSetup>(Assembly assembly)
        {
            var query = from type in assembly.ExceptionSafeGetTypes()
                        where type.Name == "Setup"
                        where typeof(TSetup).IsAssignableFrom(type)
                        select type;

            return query.FirstOrDefault();
        }

        public static Type FindSetupType<TSetup>()
        {
            var query = from assembly in AppDomain.CurrentDomain.GetAssemblies()
                        from type in assembly.ExceptionSafeGetTypes()
                        where type.Name == "Setup"
                        where typeof(TSetup).IsAssignableFrom(type)
                        select type;

            return query.FirstOrDefault();
        }
    }
}
