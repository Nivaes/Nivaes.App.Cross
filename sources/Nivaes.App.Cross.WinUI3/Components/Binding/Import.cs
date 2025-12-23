namespace Nivaes.App.Cross.WinUI3
{
    using System.Reflection;
    using MvvmCross.IoC;
    using Nivaes.App.Cross;

    public class Import
    {
        static Import()
        {
            MvxDesignTimeChecker.Check();
        }

        private object _from;

        public object From
        {
            get
            {
                return _from;
            }
            set
            {
                if (_from == value)
                    return;

                _from = value;

                if (_from != null)
                {
                    RegisterAssembly(_from.GetType().GetTypeInfo().Assembly);
                }
            }
        }

        private static void RegisterAssembly(Assembly assembly)
        {
            if (CrossSingleton<IMvxIoCProvider>.Instance == null)
            {
                MvxWindowsAssemblyCache.EnsureInitialized();
                MvxWindowsAssemblyCache.Instance?.Assemblies.Add(assembly);
            }
        }
    }
}
