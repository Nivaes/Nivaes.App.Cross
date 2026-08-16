using Microsoft.Extensions.DependencyInjection;

namespace Nivaes.App.Cross.UnitTest
{
    public class TestPlatformFixture : IDisposable
    {
        public TestPlatformFixture() 
        {
            var services = new ServiceCollection();

            services.AddSingleton<CrossViewModelLoader>();
            services.AddSingleton<CrossViewModelLocator>();
            services.AddLogging();

            IPlatformApplication.Current = new TestPlatformApplication(services.BuildServiceProvider());
        }

        public void Dispose()
        {
        }
    }
}
