//// Licensed to the .NET Foundation under one or more agreements.
//// The .NET Foundation licenses this file to you under the MS-PL license.
//// See the LICENSE file in the project root for more information.

//using Windows.ApplicationModel.Activation;

//namespace Nivaes.App.Cross.Windows
//{
//    using System;
//    using Microsoft.UI.Xaml.Controls;
//    using MvvmCross.Core;
//    using MvvmCross.Platforms.Uap.Views;

//    [Obsolete("Quitar", true)]
//    public class MvxWindowsSetupSingleton
//        : MvxSetupSingleton
//    {
//        public static MvxWindowsSetupSingleton EnsureSingletonAvailable(Frame rootFrame, IActivatedEventArgs? activatedEventArgs,
//            string? suspensionManagerSessionStateKey = null)
//        {
//            var instance = EnsureSingletonAvailable<MvxWindowsSetupSingleton>();
//            instance.PlatformSetup<MvxWindowsSetup>()?.PlatformInitialize(rootFrame, activatedEventArgs, suspensionManagerSessionStateKey);
//            return instance;
//        }

//        public static MvxWindowsSetupSingleton EnsureSingletonAvailable(Frame rootFrame, string? suspensionManagerSessionStateKey = null)
//        {
//            var instance = EnsureSingletonAvailable<MvxWindowsSetupSingleton>();
//            instance.PlatformSetup<MvxWindowsSetup>()?.PlatformInitialize(rootFrame, suspensionManagerSessionStateKey);
//            return instance;
//        }

//        public static MvxWindowsSetupSingleton EnsureSingletonAvailable(IMvxWindowsFrame rootFrame)
//        {
//            var instance = EnsureSingletonAvailable<MvxWindowsSetupSingleton>();
//            instance.PlatformSetup<MvxWindowsSetup>()?.PlatformInitialize(rootFrame);
//            return instance;
//        }
//    }
//}
