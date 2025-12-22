namespace MvvmCross.Plugin.Json
{
    using System.Diagnostics.CodeAnalysis;
    using MvvmCross.Base;
    using MvvmCross.Exceptions;
    using MvvmCross.IoC;
    using Nivaes.App.Cross;

    [MvxPlugin]
    [Preserve(AllMembers = true)]
    [RequiresUnreferencedCode("MvxJsonConverter requires unreferenced code")]
    public class Plugin : IMvxConfigurablePlugin
    {
        private MvxJsonConfiguration _configuration;

        public void Load(IMvxIoCProvider provider)
        {
            provider.RegisterType<ICrossJsonConverter, MvxJsonConverter>();
            var configuration = _configuration ?? MvxJsonConfiguration.Default;

            if (configuration.RegisterAsTextSerializer)
            {
                provider.RegisterType<ICrossTextSerializer, MvxJsonConverter>();
            }
        }

        public void Configure(IMvxPluginConfiguration configuration)
        {
            if (configuration != null && configuration is not MvxJsonConfiguration)
            {
                throw new CrossException("You must configure the Json plugin with MvxJsonConfiguration - but supplied {0}", configuration.GetType().Name);
            }

            _configuration = (MvxJsonConfiguration)configuration;
        }
    }
}
