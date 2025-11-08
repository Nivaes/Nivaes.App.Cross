namespace Nivaes.App.Cross.Droid
{
    using System.Diagnostics.CodeAnalysis;

    public class CrossAndroidSetupSingleton
        : CrossSetupSingleton
    {
        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
        public static CrossAndroidSetupSingleton EnsureSingletonAvailable(Application applicationContext)
        {
            var instance = EnsureSingletonAvailable<CrossAndroidSetupSingleton>();
            instance.PlatformSetup<CrossAndroidSetup>()?.PlatformInitialize(applicationContext);
            return instance;
        }
    }
}
