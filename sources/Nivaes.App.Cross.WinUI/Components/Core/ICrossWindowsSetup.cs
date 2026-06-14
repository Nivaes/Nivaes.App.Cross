using Microsoft.UI.Xaml.Controls;

namespace Nivaes.App.Cross.WinUI;

[Obsolete("", true)]
public interface ICrossWindowsSetup 
    : ICrossSetup
{
    void PlatformInitialize(Frame rootFrame, string activatedEventArgs, string? suspensionManagerSessionStateKey = null);
    void PlatformInitialize(Frame rootFrame, string? suspensionManagerSessionStateKey = null);
    void PlatformInitialize(ICrossWindowsFrame rootFrame);
    void UpdateActivationArguments(string e);
}
