namespace MvvmCross.Platforms.Android.Core
{
    using System.Diagnostics.CodeAnalysis;
    using Nivaes.App.Cross;

    public class MvxAndroidSetupSingleton
        : CrossSetupSingleton
    {
        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming.")]
        public static MvxAndroidSetupSingleton EnsureSingletonAvailable(Application applicationContext)
        {
            var instance = EnsureSingletonAvailable<MvxAndroidSetupSingleton>();
            instance.PlatformSetup<MvxAndroidSetup>()?.PlatformInitialize(applicationContext);
            return instance;
        }
    }
}
