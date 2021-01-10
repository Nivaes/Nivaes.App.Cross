// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Uap
{
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.IoC;
    using Windows.ApplicationModel;

    public abstract class MvxDesignTimeHelper
    {
        protected MvxDesignTimeHelper()
        {
            if (!IsInDesignTool)
                return;

            if (!IoCProvider.IsValueCreated)
            {
                var iocProvider = IoCProvider.Initialize();
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
