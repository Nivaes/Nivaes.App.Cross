﻿//// Licensed to the .NET Foundation under one or more agreements.
//// The .NET Foundation licenses this file to you under the MS-PL license.
//// See the LICENSE file in the project root for more information.

//using Android.Content;

//namespace Nivaes.App.Cross.Droid
//{
//    using System;
//    using MvvmCross.Core;
//    using Autofac;
//    using Nivaes.App.Cross.IoC;

//    [Obsolete("Eliminar")]
//    public class MvxAndroidSetupSingleton
//        : MvxSetupSingleton
//    {
//        public static MvxAndroidSetupSingleton EnsureSingletonAvailable(Context? applicationContext)
//        {
//            var instance = EnsureSingletonAvailable<MvxAndroidSetupSingleton>();
//            //instance.PlatformSetup<MvxAndroidSetup>()?.PlatformInitialize(applicationContext);

//            var setup = IoCContainer.ComponentContext.Resolve<IMvxAndroidSetup>();
//            setup?.PlatformInitialize(applicationContext);

//            return instance;
//        }
//    }
//}
