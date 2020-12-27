// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Uap
{
    using MvvmCross.IoC;
    using Nivaes.App.Cross;
    using Windows.ApplicationModel;

    public abstract class MvxDesignTimeHelper
    {
        protected MvxDesignTimeHelper()
        {
            if (!IsInDesignTool)
                return;

            if (!MvxIoCProvider.IsValueCreated)
            {
                var iocProvider = MvxIoCProvider.Initialize();
                Mvx.IoCProvider.RegisterSingleton(iocProvider);
            }
        }

        private static bool? mIsInDesignTime;

        protected static bool IsInDesignTool
        {
            get
            {
                if (!mIsInDesignTime.HasValue)
                    mIsInDesignTime = DesignMode.DesignModeEnabled;
                return mIsInDesignTime.Value;
            }
        }
    }
}
