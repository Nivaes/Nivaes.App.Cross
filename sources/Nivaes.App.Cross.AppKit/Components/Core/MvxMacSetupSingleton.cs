namespace MvvmCross.Platforms.Mac.Core
{
    using System.Diagnostics.CodeAnalysis;
    using AppKit;
    using MvvmCross.Core;
    using MvvmCross.Platforms.Mac.Presenters;
    using Nivaes.App.Cross;

    public class MvxMacSetupSingleton
        : CrossSetupSingleton
    {
        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
        public static MvxMacSetupSingleton EnsureSingletonAvailable(IMvxApplicationDelegate applicationDelegate)
        {
            var instance = EnsureSingletonAvailable<MvxMacSetupSingleton>();
            instance.PlatformSetup<MvxMacSetup>()?.PlatformInitialize(applicationDelegate);
            return instance;
        }

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
        public static MvxMacSetupSingleton EnsureSingletonAvailable(IMvxApplicationDelegate applicationDelegate, IMvxMacViewPresenter presenter)
        {
            var instance = EnsureSingletonAvailable<MvxMacSetupSingleton>();
            instance.PlatformSetup<MvxMacSetup>()?.PlatformInitialize(applicationDelegate, presenter);
            return instance;
        }
    }
}
