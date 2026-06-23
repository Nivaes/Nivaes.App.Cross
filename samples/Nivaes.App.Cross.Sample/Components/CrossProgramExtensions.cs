using Microsoft.Extensions.DependencyInjection.Extensions;
using Nivaes.App.Cross.Hosting;

namespace Nivaes.App.Cross.Sample
{
    public static class CrossProgramExtensions
    {
        public static CrossAppBuilder UseSharedCrossApp(this CrossAppBuilder builder)
        {
            builder.UseCrossApp<SampleApp>();

            builder.SetupConverters();

            return builder;
        }

        static CrossAppBuilder SetupConverters(this CrossAppBuilder builder)
        {
            CrossConvertersManagerHelper.RegisterBindersModel(new[]
            {
                CrossConvertersManagerHelper.New<StringToLowerValueConverter>(),
                CrossConvertersManagerHelper.New<StringToUpperValueConverter>(),
                CrossConvertersManagerHelper.New<TextToColorValueConverter>(),
            });

            return builder;
        }
    }
}
