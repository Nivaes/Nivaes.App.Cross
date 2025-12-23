namespace MvvmCross.Plugin.Json
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.IoC;
    using Nivaes.App.Cross;

    [MvxPlugin]
    [Preserve(AllMembers = true)]
    [RequiresUnreferencedCode("MvxJsonConverter requires unreferenced code")]
    public class Plugin : IMvxConfigurablePlugin
    {
        private CrossJsonConfiguration _configuration;

        public void Load(IMvxIoCProvider provider)
        {
            provider.RegisterType<ICrossJsonConverter, CrossJsonConverter>();
            var configuration = _configuration ?? CrossJsonConfiguration.Default;

            if (configuration.RegisterAsTextSerializer)
            {
                provider.RegisterType<ICrossTextSerializer, CrossJsonConverter>();
            }
        }

        public void Configure(IMvxPluginConfiguration configuration)
        {
            if (configuration != null && configuration is not CrossJsonConfiguration)
            {
                throw new CrossException("You must configure the Json plugin with MvxJsonConfiguration - but supplied {0}", configuration.GetType().Name);
            }

            _configuration = (CrossJsonConfiguration)configuration;
        }
    }
}
