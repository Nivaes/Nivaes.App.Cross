namespace MvvmCross.Platforms.Ios.Core
{
    using System.Diagnostics.CodeAnalysis;
    using Nivaes.App.Cross;

    public class MvxIosSetupSingleton
        : CrossSetupSingleton
    {
        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        public static MvxIosSetupSingleton EnsureSingletonAvailable(ICrossLifetime lifetimeInstance, UIWindow window)
        {
            var instance = EnsureSingletonAvailable<MvxIosSetupSingleton>();
            instance.PlatformSetup<MvxIosSetup>()?.PlatformInitialize(lifetimeInstance, window);
            return instance;
        }
    }
}
