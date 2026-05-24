using System.Text;
using Nivaes.App.Cross.Hosting;

namespace Nivaes.App.Cross.Sample.UIKitOS
{
    public static class CrossProgram
    {
        public static CrossApp CreateCrossApp()
        {
            var builder = CrossApp.CreateBuilder();

            builder
                .UseSharedCrossApp();

            return builder.Build();
        }
    }
}
