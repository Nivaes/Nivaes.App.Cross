namespace MvvmCross.Platforms.Tvos.Core
{
    using System.Diagnostics.CodeAnalysis;
    using Nivaes.App.Cross;
    using Nivaes.App.Cross.UIKit.TvOS;

    [RequiresUnreferencedCode("This class may use reflection which may not be preserved by trimming.")]
    public class MvxTvosSetupSingleton
        : CrossSetupSingleton
    {
        public static MvxTvosSetupSingleton EnsureSingletonAvailable(IMvxApplicationDelegate applicationDelegate, UIWindow window)
        {
            var instance = EnsureSingletonAvailable<MvxTvosSetupSingleton>();
            instance.PlatformSetup<MvxTvosSetup>()?.PlatformInitialize(applicationDelegate, window);
            return instance;
        }

        public static MvxTvosSetupSingleton EnsureSingletonAvailable(IMvxApplicationDelegate applicationDelegate, ICrossTvosViewPresenter presenter)
        {
            var instance = EnsureSingletonAvailable<MvxTvosSetupSingleton>();
            instance.PlatformSetup<MvxTvosSetup>()?.PlatformInitialize(applicationDelegate, presenter);
            return instance;
        }
    }
}
