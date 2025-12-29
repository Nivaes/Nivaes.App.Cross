using Microsoft.UI.Xaml.Controls;

namespace Nivaes.App.Cross.WinUI3;

public interface IMvxWindowsSetup 
    : ICrossSetup
{
    void PlatformInitialize(Frame rootFrame, string activatedEventArgs, string? suspensionManagerSessionStateKey = null);
    void PlatformInitialize(Frame rootFrame, string? suspensionManagerSessionStateKey = null);
    void PlatformInitialize(ICrossWindowsFrame rootFrame);
    void UpdateActivationArguments(string e);
}
