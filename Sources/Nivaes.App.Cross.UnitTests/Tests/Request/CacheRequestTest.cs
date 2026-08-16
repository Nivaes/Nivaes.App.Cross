namespace Nivaes.App.Cross.UnitTest
{
    public class CacheRequestTest : IClassFixture<TestPlatformFixture>
    {
        private readonly TestPlatformFixture _testPlatformFixture;

        public CacheRequestTest(TestPlatformFixture testPlatformFixture) 
        {
            _testPlatformFixture = testPlatformFixture;
        }

        [Fact]
        public void IncludeCacheTest()
        {
            #region Load
            for (int i = 0; i < 10; i++)
            {
                var request = new ViewModelRequest<MoqViewModel>();
                request.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
                var id = ViewModelRequestCache.Add(request.ViewModel);
            }

            var request1 = new ViewModelRequest<MoqViewModel>()
            {
                ParameterValues = new Dictionary<string, string> {["Uno"] = "Uno" }
            };
            request1.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
            var id1 = ViewModelRequestCache.Add(request1.ViewModel);

            var request2 = new ViewModelRequest<MoqViewModel>();
            request2.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
            var id2 = ViewModelRequestCache.Add(request2.ViewModel);

            for (int i = 0; i < 5; i++)
            {
                var request = new ViewModelRequest<MoqViewModel>();
                request.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
                var id = ViewModelRequestCache.Add(request.ViewModel);
            }

            var request3 = new ViewModelRequest<MoqViewModel>();
            request3.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
            var id3 = ViewModelRequestCache.Add(request3.ViewModel);
            #endregion

            ViewModelRequestCache.TryGetValue(id1, out var viewModel1_recover).ShouldBeTrue();
            viewModel1_recover.ShouldNotBeNull();
            viewModel1_recover.GetHashCode().ShouldBe(request1.ViewModel.GetHashCode());
        }

        [Fact]
        public void IncludeCacheSearchByReferenceTest()
        {
            #region Load
            for (int i = 0; i < 10; i++)
            {
                var request = new ViewModelRequest<MoqViewModel>();
                request.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
                var id = ViewModelRequestCache.Add(request.ViewModel);
            }

            var request1 = new ViewModelRequest<MoqViewModel>()
            {
                ParameterValues = new Dictionary<string, string> { ["Uno"] = "Uno" }
            };
            request1.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
            var id1 = ViewModelRequestCache.Add(request1.ViewModel);

            var request2 = new ViewModelRequest<MoqViewModel>();
            request2.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
            var id2 = ViewModelRequestCache.Add(request2.ViewModel);

            for (int i = 0; i < 5; i++)
            {
                var request = new ViewModelRequest<MoqViewModel>();
                request.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
                var id = ViewModelRequestCache.Add(request.ViewModel);
            }

            var request3 = new ViewModelRequest<MoqViewModel>();
            request3.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
            var id3 = ViewModelRequestCache.Add(request3.ViewModel);
            #endregion

            ViewModelRequestCache.TryGetValue(request1.ViewModel, out var id_recover).ShouldBeTrue();
            id_recover.ShouldNotBeNull();
            id_recover.ShouldBe(id1);
        }

        [Fact]
        public void IncludeCacheNotFoundTest()
        {
            #region Load
            for (int i = 0; i < 10; i++)
            {
                var request = new ViewModelRequest<MoqViewModel>();
                request.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
                var id = ViewModelRequestCache.Add(request.ViewModel);
            }

            var request1 = new ViewModelRequest<MoqViewModel>();
            request1.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
            var id1 = ViewModelRequestCache.Add(request1.ViewModel);

            var request2 = new ViewModelRequest<MoqViewModel>();
            request2.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
            var id2 = ViewModelRequestCache.Add(request2.ViewModel);

            for (int i = 0; i < 5; i++)
            {
                var request = new ViewModelRequest<MoqViewModel>();
                request.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
                var id = ViewModelRequestCache.Add(request.ViewModel);
            }

            var request3 = new ViewModelRequest<MoqViewModel>();
            request3.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
            var id3 = ViewModelRequestCache.Add(request3.ViewModel);
            #endregion

            ViewModelRequestCache.TryGetValue(10000, out var request1_recover).ShouldBeFalse();
        }

        [Fact]
        public void IncludeCacheDeleteItemTest()
        {
            #region Load
            for (int i = 0; i < 10; i++)
            {
                var request = new ViewModelRequest<MoqViewModel>();
                request.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
                var id = ViewModelRequestCache.Add(request.ViewModel);
            }

            var request1 = new ViewModelRequest<MoqViewModel>();
            request1.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
            var id1 = ViewModelRequestCache.Add(request1.ViewModel);

            var request2 = new ViewModelRequest<MoqViewModel>();
            request2.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
            var id2 = ViewModelRequestCache.Add(request2.ViewModel);

            for (int i = 0; i < 5; i++)
            {
                var request = new ViewModelRequest<MoqViewModel>();
                request.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
                var id = ViewModelRequestCache.Add(request.ViewModel);
            }

            var request3 = new ViewModelRequest<MoqViewModel>();
            request3.ViewModel.GetType().ShouldBe(typeof(MoqViewModel));
            var id3 = ViewModelRequestCache.Add(request3.ViewModel);
            #endregion

            ViewModelRequestCache.Delete(id2).ShouldBeTrue();
            ViewModelRequestCache.Delete(id2).ShouldBeFalse();
            ViewModelRequestCache.TryGetValue(id2, out var viewModel2_recover).ShouldBeFalse();
            viewModel2_recover.ShouldBeNull();

            ViewModelRequestCache.TryGetValue(id1, out var viewModel1_recover).ShouldBeTrue();
            viewModel1_recover.ShouldNotBeNull();
            viewModel1_recover!.GetHashCode().ShouldBe(request1.ViewModel.GetHashCode());

        }
    }
}
