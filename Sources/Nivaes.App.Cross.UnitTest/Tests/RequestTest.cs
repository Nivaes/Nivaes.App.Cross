using System.Security.Cryptography.X509Certificates;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Nivaes.App.Cross.UnitTest
{
    public class RequestTest
    {
        [Fact]
        public void RequestGenerationTest()
        {
            var services = new ServiceCollection();

            services.AddSingleton<CrossViewModelLoader>();
            services.AddSingleton<CrossViewModelLocator>();
            services.AddLogging();

            IPlatformApplication.Current =
                new TestPlatformApplication(services.BuildServiceProvider());

            var viewModelLoader = IPlatformApplication.Current!.ServiceProvider.GetRequiredService<CrossViewModelLoader>();

            var request = new ViewModelRequest(typeof(MoqViewModel))
            {

            };

            request.ShouldNotBeNull();
            request.ViewModel.ShouldNotBeNull();
            request.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
        }
    }
}
