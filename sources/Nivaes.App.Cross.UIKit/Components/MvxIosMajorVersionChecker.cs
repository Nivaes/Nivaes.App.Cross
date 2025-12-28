using Microsoft.Extensions.Logging;
using Nivaes.IoC;

namespace Nivaes.App.Cross.UIKitOS
{   
    public class MvxIosMajorVersionChecker
    {
        public bool IsVersionOrHigher { get; private set; }

        public MvxIosMajorVersionChecker(int major, bool defaultValue = true)
        {
            IsVersionOrHigher = ReadIsIosVersionOrHigher(major, defaultValue);
        }

        private static bool ReadIsIosVersionOrHigher(int target, bool defaultValue)
        {
            if (Mvx.IoCProvider?.TryResolve(out IMvxIosSystem? iosSystem) != true)
            {
                CrossLogHost.Default?.LogWarning(
                    "IMvxIosSystem not found - so assuming we {Target} on iOS {Default} or later", target, defaultValue ? "are" : "are not");
                return defaultValue;
            }

            return iosSystem?.Version.Major >= target;
        }
    }
}
