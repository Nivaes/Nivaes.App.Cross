namespace Nivaes.App.Cross.UnitTest.Request
{
    public class RequestTest : IClassFixture<TestPlatformFixture>
    {
        private readonly TestPlatformFixture _testPlatformFixture;

        public RequestTest(TestPlatformFixture testPlatformFixture) 
        {
            _testPlatformFixture = testPlatformFixture;
        }

        [Fact]
        public void RequestGenerationTest()
        {
            var request = new ViewModelRequest<MoqViewModel>()
            {

            };

            request.ShouldNotBeNull();
            request.ViewModel.ShouldNotBeNull();
            request.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
        }
    }
}
