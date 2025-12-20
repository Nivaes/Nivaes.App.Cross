//namespace Nivaes.App.Cross.WinUI3
//{
//    using System.Reflection;
//    using MvvmCross.IoC;

//    [Obsolete("Quitar IoC de Cross", true)]
//    public class Import
//    {
//        static Import()
//        {
//            CrossDesignTimeChecker.Check();
//        }

//        private object _from;

//        public object From
//        {
//            get
//            {
//                return _from;
//            }
//            set
//            {
//                if (_from == value)
//                    return;

//                _from = value;

//                if (_from != null)
//                {
//                    RegisterAssembly(_from.GetType().GetTypeInfo().Assembly);
//                }
//            }
//        }

//        private static void RegisterAssembly(Assembly assembly)
//        {
//            if (CrossSingleton<ICrossIoCProvider>.Instance == null)
//            {
//                CrossWindowsAssemblyCache.EnsureInitialized();
//                CrossWindowsAssemblyCache.Instance?.Assemblies.Add(assembly);
//            }
//        }
//    }
//}
