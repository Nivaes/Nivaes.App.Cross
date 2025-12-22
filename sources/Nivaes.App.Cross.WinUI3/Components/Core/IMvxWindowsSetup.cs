namespace MvvmCross.Platforms.WinUi.Core
{
    using Microsoft.UI.Xaml.Controls;
    using MvvmCross.Platforms.WinUi.Views;
    using Nivaes.App.Cross;

    public interface IMvxWindowsSetup 
        : ICrossSetup
    {
        void PlatformInitialize(Frame rootFrame, string activatedEventArgs, string? suspensionManagerSessionStateKey = null);
        void PlatformInitialize(Frame rootFrame, string? suspensionManagerSessionStateKey = null);
        void PlatformInitialize(IMvxWindowsFrame rootFrame);
        void UpdateActivationArguments(string e);
    }
}
