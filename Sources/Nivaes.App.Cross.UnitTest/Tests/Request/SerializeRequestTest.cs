namespace Nivaes.App.Cross.UnitTest.Request
{
    public class SerializeRequestTest : IClassFixture<TestPlatformFixture>
    {
        private readonly TestPlatformFixture _testPlatformFixture;

        public SerializeRequestTest(TestPlatformFixture testPlatformFixture) 
        {
            _testPlatformFixture = testPlatformFixture;
        }

        [Fact]
        public void SerializaRequestTest()
        {
            var request = new ViewModelRequest(typeof(MoqViewModel))
            {

            };
            request.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));

            var buffer = ViewModelRequestSerializer.Serializer(request);
            buffer.ShouldNotBeNull();
            buffer.Length.ShouldBeGreaterThan(0);

            var requestCopy = ViewModelRequestSerializer.Deserialize(buffer);
            requestCopy.ShouldNotBeNull();
            requestCopy.ViewModelType.ShouldBe(typeof(MoqViewModel));
        }

        [Fact]
        public void SerializaRequestMultiplesTest()
        {
            #region Load
            for (int i = 0; i < 10; i++)
            {
                var request = new ViewModelRequest(typeof(MoqViewModel));
                request.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
                var buffer = ViewModelRequestSerializer.Serializer(request);
                buffer.ShouldNotBeNull();
                buffer.Length.ShouldBeGreaterThan(0);
            }

            var request1 = new ViewModelRequest(typeof(MoqViewModel));
            request1.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
            var buffer1 = ViewModelRequestSerializer.Serializer(request1);
            buffer1.ShouldNotBeNull();
            buffer1.Length.ShouldBeGreaterThan(0);

            var request2 = new ViewModelRequest(typeof(MoqViewModel));
            request1.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
            var buffer2 = ViewModelRequestSerializer.Serializer(request2);
            buffer2.ShouldNotBeNull();
            buffer2.Length.ShouldBeGreaterThan(0);

            for (int i = 0; i < 4; i++)
            {
                var request = new ViewModelRequest(typeof(MoqViewModel));
                request.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
                var buffer = ViewModelRequestSerializer.Serializer(request);
                buffer.ShouldNotBeNull();
                buffer.Length.ShouldBeGreaterThan(0);
            }

            var request3 = new ViewModelRequest(typeof(MoqViewModel));
            request1.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
            var buffer3 = ViewModelRequestSerializer.Serializer(request3);
            buffer3.ShouldNotBeNull();
            buffer3.Length.ShouldBeGreaterThan(0);

            var request4 = new ViewModelRequest(typeof(MoqViewModel));
            request1.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
            var buffer4 = ViewModelRequestSerializer.Serializer(request4);
            buffer1.ShouldNotBeNull();
            buffer1.Length.ShouldBeGreaterThan(0);
            #endregion

            var request1Copy = ViewModelRequestSerializer.Deserialize(buffer1);
            request1Copy.ShouldNotBeNull();
            request1Copy.ViewModelType.ShouldBe(typeof(MoqViewModel));
            request1Copy.ViewModel.ShouldBe(request1.ViewModel);
            request1Copy.ViewModel.GetHashCode().ShouldBe(request1.ViewModel.GetHashCode());
            request1Copy.ViewModelType.ShouldBe(request1.ViewModelType);
        }

        [Fact]
        public void SerializaRequestMultiplesAndDeleteTest()
        {
            #region Load
            for (int i = 0; i < 10; i++)
            {
                var request = new ViewModelRequest(typeof(MoqViewModel));
                request.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
                var buffer = ViewModelRequestSerializer.Serializer(request);
                buffer.ShouldNotBeNull();
                buffer.Length.ShouldBeGreaterThan(0);
            }

            var request1 = new ViewModelRequest(typeof(MoqViewModel));
            request1.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
            var buffer1 = ViewModelRequestSerializer.Serializer(request1);
            buffer1.ShouldNotBeNull();
            buffer1.Length.ShouldBeGreaterThan(0);

            var request2 = new ViewModelRequest(typeof(MoqViewModel));
            request1.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
            var buffer2 = ViewModelRequestSerializer.Serializer(request2);
            buffer2.ShouldNotBeNull();
            buffer2.Length.ShouldBeGreaterThan(0);

            for (int i = 0; i < 4; i++)
            {
                var request = new ViewModelRequest(typeof(MoqViewModel));
                request.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
                var buffer = ViewModelRequestSerializer.Serializer(request);
                buffer.ShouldNotBeNull();
                buffer.Length.ShouldBeGreaterThan(0);
            }

            var request3 = new ViewModelRequest(typeof(MoqViewModel));
            request1.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
            var buffer3 = ViewModelRequestSerializer.Serializer(request3);
            buffer3.ShouldNotBeNull();
            buffer3.Length.ShouldBeGreaterThan(0);

            var request4 = new ViewModelRequest(typeof(MoqViewModel));
            request1.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
            var buffer4 = ViewModelRequestSerializer.Serializer(request4);
            buffer1.ShouldNotBeNull();
            buffer1.Length.ShouldBeGreaterThan(0);
            #endregion

            var request1Copy = ViewModelRequestSerializer.Deserialize(buffer1);
            request1Copy.ShouldNotBeNull();
            request1Copy.ViewModelType.ShouldBe(typeof(MoqViewModel));
            request1Copy.ViewModel.ShouldBe(request1.ViewModel);
            request1Copy.ViewModel.GetHashCode().ShouldBe(request1.ViewModel.GetHashCode());
            request1Copy.ViewModelType.ShouldBe(request1.ViewModelType);

            var id = ViewModelRequestSerializer.DeserializeId(buffer1);
            ViewModelRequestCache.Delete(id);

            Should.Throw<AppException>(()=>
            {
                var request2Copy = ViewModelRequestSerializer.Deserialize(buffer1);
            } );
        }
    }
}
