namespace Nivaes.App.Cross.Droid
{
    using MvvmCross.IoC;

    [Obsolete("", true)]
    [MvxPlugin]
    [Preserve(AllMembers = true)]
    public sealed class Plugin : CrossBasePlugin
    {
        public override void Load(IMvxIoCProvider provider)
        {
            provider.RegisterSingleton<ICrossNativeColor>(new MvxAndroidColor());
            RegisterDefaultBindings(provider);
            base.Load(provider);
        }

        private static void RegisterDefaultBindings(IMvxIoCProvider provider)
        {
            //var helper = new CrossDefaultColorBindingHelper();
            //helper.RegisterBindings(/*provider*/);
        }
    }
}
