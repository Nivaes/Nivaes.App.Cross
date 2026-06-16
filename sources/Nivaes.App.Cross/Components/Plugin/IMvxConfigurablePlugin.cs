namespace Nivaes.App.Cross
{
    [Obsolete("", true)]
    public interface IMvxConfigurablePlugin : IMvxPlugin
    {
        void Configure(IMvxPluginConfiguration configuration);
    }
}
