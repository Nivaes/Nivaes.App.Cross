//namespace Nivaes.App.Cross.AppKitOS
//{
//    using System.Diagnostics.CodeAnalysis;
//    using Nivaes.App.Cross.AppKit.Components.Hosting;

//    public class MvxMacSetupSingleton
//        : CrossSetupSingleton
//    {
//        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
//        public static MvxMacSetupSingleton EnsureSingletonAvailable(IMvxApplicationDelegate applicationDelegate)
//        {
//            var instance = EnsureSingletonAvailable<MvxMacSetupSingleton>();
//            instance.PlatformSetup<MvxMacSetup>()?.PlatformInitialize(applicationDelegate);
//            return instance;
//        }

//        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
//        public static MvxMacSetupSingleton EnsureSingletonAvailable(IMvxApplicationDelegate applicationDelegate, IMvxMacViewPresenter presenter)
//        {
//            var instance = EnsureSingletonAvailable<MvxMacSetupSingleton>();
//            instance.PlatformSetup<MvxMacSetup>()?.PlatformInitialize(applicationDelegate, presenter);
//            return instance;
//        }
//    }
//}
