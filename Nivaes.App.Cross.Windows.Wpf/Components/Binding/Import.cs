//// Licensed to the .NET Foundation under one or more agreements.
//// The .NET Foundation licenses this file to you under the MS-PL license.
//// See the LICENSE file in the project root for more information.

//namespace MvvmCross.Platforms.Wpf.Binding
//{
//    using System;
//    using System.Reflection;
//    using MvvmCross.Binding.Combiners;
//    using MvvmCross.Converters;
//    using MvvmCross.IoC;
//    using Nivaes.App.Cross;
//    using Nivaes.App.Cross.IoC;

//    [Obsolete("Not user reflector.", true)]
//    public class Import
//    {
//        static Import()
//        {
//            MvxDesignTimeChecker.Check();
//        }

//        private object mFrom;

//        public object From
//        {
//            get => mFrom;
//            set
//            {
//                if (mFrom == value)
//                    return;

//                mFrom = value;
//                if (mFrom != null)
//                {
//                    RegisterAssembly(mFrom.GetType().Assembly);
//                }
//            }
//        }

//        [Obsolete("Not user reflector", true)]
//        private static void RegisterAssembly(Assembly assembly)
//        {
//            if (!IoCProvider.IsValueCreated)
//            {
//                MvxWindowsAssemblyCache.EnsureInitialized();
//                MvxWindowsAssemblyCache.Instance?.Assemblies.Add(assembly);
//            }
//            else
//            {
//                Mvx.IoCProvider.CallbackWhenRegistered<IMvxValueConverterRegistry>(
//                    registry =>
//                        {
//                            registry.AddOrOverwriteFrom(assembly);
//                        });

//                Mvx.IoCProvider.CallbackWhenRegistered<IMvxValueCombinerRegistry>(
//                    registry =>
//                        {
//                            registry.AddOrOverwriteFrom(assembly);
//                        });
//            }
//        }
//    }
//}
