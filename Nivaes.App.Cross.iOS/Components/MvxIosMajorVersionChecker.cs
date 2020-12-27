// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.Platforms.Ios
{
    using Nivaes.App.Cross.Logging;
    using Nivaes.App.Cross;

    public class MvxIosMajorVersionChecker
    {
        public bool IsVersionOrHigher { get; private set; }

        public MvxIosMajorVersionChecker(int major, bool defaultValue = true)
        {
            IsVersionOrHigher = ReadIsIosVersionOrHigher(major, defaultValue);
        }

        private static bool ReadIsIosVersionOrHigher(int target, bool defaultValue)
        {
            Mvx.IoCProvider.TryResolve<IMvxIosSystem>(out IMvxIosSystem iosSystem);
            if (iosSystem == null)
            {
                MvxLog.Instance?.Warn("IMvxIosSystem not found - so assuming we {1} on iOS {0} or later", target, defaultValue ? "are" : "are not");
                return defaultValue;
            }

            return iosSystem.Version.Major >= target;
        }
    }
}
