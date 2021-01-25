//// Licensed to the .NET Foundation under one or more agreements.
//// The .NET Foundation licenses this file to you under the MS-PL license.
//// See the LICENSE file in the project root for more information.

//namespace MvvmCross.Platforms.Ios.Core
//{
//    using MvvmCross.Core;
//    using Nivaes.App.Cross.Presenters;
//    using UIKit;

//    public class MvxIosSetupSingleton
//        : MvxSetupSingleton
//    {
//        public static MvxIosSetupSingleton EnsureSingletonAvailable(IMvxApplicationDelegate applicationDelegate, UIWindow window)
//        {
//            var instance = EnsureSingletonAvailable<MvxIosSetupSingleton>();
//            instance.PlatformSetup<MvxIosSetup>()?.PlatformInitialize(applicationDelegate, window);
//            return instance;
//        }

//        public static MvxIosSetupSingleton EnsureSingletonAvailable(IMvxApplicationDelegate applicationDelegate, IMvxIosViewPresenter presenter)
//        {
//            var instance = EnsureSingletonAvailable<MvxIosSetupSingleton>();
//            instance.PlatformSetup<MvxIosSetup>()?.PlatformInitialize(applicationDelegate, presenter);
//            return instance;
//        }
//    }
//}
