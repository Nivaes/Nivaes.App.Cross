namespace Nivaes.App.Cross.WinUI3
{
    using System.Reflection;

    [Obsolete("No compatible con IoC", true)]
    public class CrossWindowsAssemblyCache
        : CrossSingleton<ICrossWindowsAssemblyCache>, ICrossWindowsAssemblyCache
    {
        public static void EnsureInitialized()
        {
            if (Instance != null)
                return;

            var instance = new CrossWindowsAssemblyCache();

            if (Instance != instance)
                throw new CrossException("Error initialising CrossWindowsAssemblyCache");
        }

        public CrossWindowsAssemblyCache()
        {
            Assemblies = new List<Assembly>();
        }

        public IList<Assembly> Assemblies { get; }
    }
}
