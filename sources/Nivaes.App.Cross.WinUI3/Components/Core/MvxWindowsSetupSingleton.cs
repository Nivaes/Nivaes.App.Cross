namespace Nivaes.App.Cross.WinUI3
{
    using Microsoft.UI.Xaml.Controls;
    using Nivaes.App.Cross;

    public class MvxWindowsSetupSingleton
        : CrossSetupSingleton
    {
        public static MvxWindowsSetupSingleton EnsureSingletonAvailable(Frame rootFrame, string activatedEventArgs,
          string? suspensionManagerSessionStateKey = null)
        {
            var instance = EnsureSingletonAvailable<MvxWindowsSetupSingleton>();
            instance.PlatformSetup<MvxWindowsSetup>()?.PlatformInitialize(rootFrame, activatedEventArgs, suspensionManagerSessionStateKey);
            return instance;
        }

        public static MvxWindowsSetupSingleton EnsureSingletonAvailable(Frame rootFrame, string? suspensionManagerSessionStateKey = null)
        {
            var instance = EnsureSingletonAvailable<MvxWindowsSetupSingleton>();
            instance.PlatformSetup<MvxWindowsSetup>()?.PlatformInitialize(rootFrame, suspensionManagerSessionStateKey);
            return instance;
        }

        public static MvxWindowsSetupSingleton EnsureSingletonAvailable(IMvxWindowsFrame rootFrame)
        {
            var instance = EnsureSingletonAvailable<MvxWindowsSetupSingleton>();
            instance.PlatformSetup<MvxWindowsSetup>()?.PlatformInitialize(rootFrame);
            return instance;
        }
    }
}
