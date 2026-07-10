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
    }
}
