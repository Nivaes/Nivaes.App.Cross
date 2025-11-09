namespace Nivaes.App.Cross.UIKit
{
    using System.Diagnostics.CodeAnalysis;

    public class CrossIosSetupSingleton
        : CrossSetupSingleton
    {
        [RequiresUnreferencedCode("This method uses reflection to check for referenced assemblies, which may not be preserved by trimming")]
        public static CrossIosSetupSingleton EnsureSingletonAvailable(ICrossLifetime lifetimeInstance, UIWindow window)
        {
            var instance = EnsureSingletonAvailable<CrossIosSetupSingleton>();
            instance.PlatformSetup<CrossIosSetup>()?.PlatformInitialize(lifetimeInstance, window);
            return instance;
        }
    }
}
