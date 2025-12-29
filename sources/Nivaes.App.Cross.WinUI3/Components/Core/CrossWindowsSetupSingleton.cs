using Microsoft.UI.Xaml.Controls;

namespace Nivaes.App.Cross.WinUI3;

public class CrossWindowsSetupSingleton
    : CrossSetupSingleton
{
    public static CrossWindowsSetupSingleton EnsureSingletonAvailable(Frame rootFrame, string activatedEventArgs,
      string? suspensionManagerSessionStateKey = null)
    {
        var instance = EnsureSingletonAvailable<CrossWindowsSetupSingleton>();
        instance.PlatformSetup<CrossWindowsSetup>()?.PlatformInitialize(rootFrame, activatedEventArgs, suspensionManagerSessionStateKey);
        return instance;
    }

    public static CrossWindowsSetupSingleton EnsureSingletonAvailable(Frame rootFrame, string? suspensionManagerSessionStateKey = null)
    {
        var instance = EnsureSingletonAvailable<CrossWindowsSetupSingleton>();
        instance.PlatformSetup<CrossWindowsSetup>()?.PlatformInitialize(rootFrame, suspensionManagerSessionStateKey);
        return instance;
    }

    public static CrossWindowsSetupSingleton EnsureSingletonAvailable(ICrossWindowsFrame rootFrame)
    {
        var instance = EnsureSingletonAvailable<CrossWindowsSetupSingleton>();
        instance.PlatformSetup<CrossWindowsSetup>()?.PlatformInitialize(rootFrame);
        return instance;
    }
}
