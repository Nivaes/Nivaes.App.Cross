// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Wpf
{
    using System.ComponentModel;
    using System.Windows;
    using System.Windows.Controls;
    using MvvmCross.Core;
    using MvvmCross.IoC;
    using MvvmCross.Platforms.Wpf.Core;
    using Nivaes.App.Cross;

    internal static class MvxDesignTimeHelper
    {
        private static bool? mIsInDesignTime;

        public static bool IsInDesignTime
        {
            get
            {
                if (!mIsInDesignTime.HasValue)
                {
                    mIsInDesignTime =
                        (bool)
                        DesignerProperties.IsInDesignModeProperty
                            .GetMetadata(typeof(DependencyObject))
                            .DefaultValue;
                }

                return mIsInDesignTime.Value;
            }
        }

        public static void Initialize()
        {
            if (!IsInDesignTime)
                return;

            if (!MvxIoCProvider.IsValueCreated)
            {
                var iocProvider = MvxIoCProvider.Initialize();
                Mvx.IoCProvider.RegisterSingleton(iocProvider);
            }

            MvxSetup.RegisterSetupType<MvxWpfSetup<App>>(System.Reflection.Assembly.GetExecutingAssembly());
            var instance = MvxWpfSetupSingleton.EnsureSingletonAvailable(Application.Current.Dispatcher, new Content());
        }

        class App : ViewModels.MvxApplication
        {
        }

        class Content : ContentControl
        {
        }
    }
}
