namespace Nivaes.App.Cross.Droid.Components.Plugins
{
    using MvvmCross;
    using MvvmCross.IoC;
    using MvvmCross.Plugin;
    using MvvmCross.Plugin.Color;
    using MvvmCross.Plugin.Color.Platforms.Android;
    using MvvmCross.Plugin.Color.Platforms.Android.BindingTargets;

    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public sealed class Plugin : BasePlugin
    {
        public override void Load(IMvxIoCProvider provider)
        {
            provider.RegisterSingleton<ICrossNativeColor>(new MvxAndroidColor());
            RegisterDefaultBindings(provider);
            base.Load(provider);
        }

        private static void RegisterDefaultBindings(IMvxIoCProvider provider)
        {
            var helper = new MvxDefaultColorBindingSet();
            helper.RegisterBindings(provider);
        }
    }
}
