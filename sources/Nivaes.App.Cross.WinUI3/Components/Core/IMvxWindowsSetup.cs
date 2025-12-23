namespace Nivaes.App.Cross.WinUI3
{
    using Microsoft.UI.Xaml.Controls;
    using MvvmCross.Platforms.WinUi.Views;

    public interface IMvxWindowsSetup 
        : ICrossSetup
    {
        void PlatformInitialize(Frame rootFrame, string activatedEventArgs, string? suspensionManagerSessionStateKey = null);
        void PlatformInitialize(Frame rootFrame, string? suspensionManagerSessionStateKey = null);
        void PlatformInitialize(IMvxWindowsFrame rootFrame);
        void UpdateActivationArguments(string e);
    }
}
