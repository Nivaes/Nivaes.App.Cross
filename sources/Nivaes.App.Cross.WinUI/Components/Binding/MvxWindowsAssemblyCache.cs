namespace Nivaes.App.Cross.WinUI3
{
    using System.Collections.Generic;
    using System.Reflection;
    using Nivaes.App.Cross;

    public class MvxWindowsAssemblyCache
        : CrossSingleton<IMvxWindowsAssemblyCache>, IMvxWindowsAssemblyCache
    {
        public static void EnsureInitialized()
        {
            if (Instance != null)
                return;

            var instance = new MvxWindowsAssemblyCache();

            if (Instance != instance)
                throw new CrossException("Error initialising MvxWindowsAssemblyCache");
        }

        public MvxWindowsAssemblyCache()
        {
            Assemblies = new List<Assembly>();
        }

        public IList<Assembly> Assemblies { get; }
    }
}
