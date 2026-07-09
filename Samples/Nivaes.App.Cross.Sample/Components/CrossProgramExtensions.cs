using Nivaes.App.Cross.Hosting;

namespace Nivaes.App.Cross.Sample
{
    public static class CrossProgramExtensions
    {
        public static CrossAppBuilder UseSharedCrossApp(this CrossAppBuilder builder)
        {
            builder.UseCrossApp<SampleApp>();

            return builder;
        }

        [Obsolete("", true)]
        public static IServiceProvider SetupConverters(this IServiceProvider services)
        {
            CrossConvertersManagerHelper.RegisterComverters(new[]
            {
                CrossConvertersManagerHelper.New<StringToLowerValueConverter>(services),
                CrossConvertersManagerHelper.New<StringToUpperValueConverter>(services),
                CrossConvertersManagerHelper.New<TextToColorValueConverter>(services),
            });

            return services;
        }
    }
}
