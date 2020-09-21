// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.UnitTest.ViewModels
{
    using System;
    using Moq;
    using MvvmCross.Logging;
    using Nivaes.App.Cross.UnitTest;
    using Xunit;

    [Collection("MvxTest")]
    public class MvxNavigationViewModelTest
    {
        private readonly NavigationTestFixture mFixture;

        public MvxNavigationViewModelTest(NavigationTestFixture fixture)
        {
            mFixture = fixture;

            var logProvider = new Mock<IMvxLogProvider>();
            logProvider.Setup(
                l => l.GetLogFor(It.IsAny<Type>()))
                      .Returns(() =>
                      {
                          var vm = new Mock<IMvxLog>();
                          return vm.Object;
                      });
            mFixture.Ioc.RegisterSingleton(logProvider.Object);

            mFixture.Ioc.RegisterType<NavigationTestViewModel, NavigationTestViewModel>();
        }

        [Fact]
        public void TestRoundTrip()
        {
            var navViewModel = mFixture.Ioc.Resolve<NavigationTestViewModel>();
            Assert.NotNull(navViewModel);
            Assert.NotNull(navViewModel.NavService);
            Assert.NotNull(navViewModel.LoggingProvider);
            Assert.NotNull(navViewModel.ViewModelLog);
        }
    }
}
