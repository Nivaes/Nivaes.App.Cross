namespace MvvmCross.Platforms.WinUi.Binding
{
    using System.Collections.Generic;
    using System.Reflection;
    using MvvmCross.Base;
    using MvvmCross.Exceptions;
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
                throw new MvxException("Error initialising MvxWindowsAssemblyCache");
        }

        public MvxWindowsAssemblyCache()
        {
            Assemblies = new List<Assembly>();
        }

        public IList<Assembly> Assemblies { get; }
    }
}
