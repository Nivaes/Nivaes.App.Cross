namespace Nivaes.App.Cross
{
    public interface IMvxConfigurablePlugin : IMvxPlugin
    {
        void Configure(IMvxPluginConfiguration configuration);
    }
}
