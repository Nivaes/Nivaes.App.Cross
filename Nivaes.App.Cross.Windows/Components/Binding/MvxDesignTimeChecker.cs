// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Uap.Binding
{
    using MvvmCross.Binding.Parse.Binding;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.IoC;
    using Windows.ApplicationModel;

    public static class MvxDesignTimeChecker
    {
        private static bool mChecked;

        public static void Check()
        {
            if (mChecked)
                return;

            mChecked = true;

            if (!DesignMode.DesignModeEnabled)
                return;

            if (!IoCProvider.IsValueCreated)
            {
                var iocProvider = IoCProvider.Initialize();
                Mvx.IoCProvider.RegisterSingleton(iocProvider);
            }

            if (!Mvx.IoCProvider.CanResolve<IMvxBindingParser>())
            {
                var builder = new MvxWindowsBindingBuilder(MvxWindowsBindingBuilder.BindingType.MvvmCross);
                builder.DoRegistration();
            }
        }
    }
}
