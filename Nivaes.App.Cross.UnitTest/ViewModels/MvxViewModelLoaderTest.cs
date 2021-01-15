﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.UnitTest.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Moq;
    using MvvmCross.Exceptions;
    using MvvmCross.UnitTest.Mocks.TestViewModels;
    using Nivaes.App.Cross.Navigation;
    using Nivaes.App.Cross.ViewModels;
    using Xunit;

    [Collection("MvxTest")]
    public class MvxViewModelLoaderTest
    {
        private readonly NavigationTestFixture _fixture;

        public MvxViewModelLoaderTest(NavigationTestFixture fixture)
        {
            _fixture = fixture;
        }


        [Fact]
        public void Test_LoaderForNull()
        {
            _fixture.ClearAll();

            var request = new MvxViewModelRequest<MvxNullViewModel>(null, null);
            var state = new MvxBundle();
            var loader = new MvxViewModelLoader(null);
            var args = new MvxNavigateEventArgs(NavigationMode.Show);
            var viewModel = loader.LoadViewModel(request, state, args);

            Assert.IsType<MvxNullViewModel>(viewModel);
        }

        [Fact]
        public async Task Test_NormalViewModel()
        {
            _fixture.ClearAll();

            IMvxViewModel outViewModel = new Test2ViewModel();

            var mockLocator = new Mock<IMvxViewModelLocator>();
            mockLocator.Setup(
                m => m.Load(It.IsAny<Type>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxNavigateEventArgs>()))
                       .Returns(() => new ValueTask<IMvxViewModel>(outViewModel));

            var mockCollection = new Mock<IMvxViewModelLocatorCollection>();
            mockCollection.Setup(m => m.FindViewModelLocator(It.IsAny<MvxViewModelRequest>()))
                          .Returns(() => mockLocator.Object);

            var parameters = new Dictionary<string, string> { { "foo", "bar" } };
            var request = new MvxViewModelRequest<Test2ViewModel>(new MvxBundle(parameters), null);
            var state = new MvxBundle();
            var loader = new MvxViewModelLoader(mockCollection.Object);
            var args = new MvxNavigateEventArgs(NavigationMode.Show);
            var viewModel = await loader.LoadViewModel(request, state, args).ConfigureAwait(true);

            Assert.Equal(outViewModel, viewModel);
        }

        [Fact]
        public void Test_FailedViewModel()
        {
            _fixture.ClearAll();

            var mockLocator = new Mock<IMvxViewModelLocator>();
            mockLocator.Setup(
                m => m.Load(It.IsAny<Type>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxBundle>(), It.IsAny<IMvxNavigateEventArgs>()))
                       .Throws<MvxException>();

            var mockCollection = new Mock<IMvxViewModelLocatorCollection>();
            mockCollection.Setup(m => m.FindViewModelLocator(It.IsAny<MvxViewModelRequest>()))
                          .Returns(() => mockLocator.Object);

            var parameters = new Dictionary<string, string> { { "foo", "bar" } };
            var request = new MvxViewModelRequest<Test2ViewModel>(new MvxBundle(parameters), null);
            var state = new MvxBundle();
            var loader = new MvxViewModelLoader(mockCollection.Object);
            var args = new MvxNavigateEventArgs(NavigationMode.Show);
            Assert.Throws<MvxException>(() =>
            {
                var viewModel = loader.LoadViewModel(request, state, args);
            });
        }

        [Fact]
        public void Test_FailedViewModelLocatorCollection()
        {
            _fixture.ClearAll();

            var mockCollection = new Mock<IMvxViewModelLocatorCollection>();
            mockCollection.Setup(m => m.FindViewModelLocator(It.IsAny<MvxViewModelRequest>()))
                          .Returns(() => null);

            var parameters = new Dictionary<string, string> { { "foo", "bar" } };
            var request = new MvxViewModelRequest<Test2ViewModel>(new MvxBundle(parameters), null);
            var state = new MvxBundle();
            var loader = new MvxViewModelLoader(mockCollection.Object);
            var args = new MvxNavigateEventArgs(NavigationMode.Show);
            Assert.Throws<MvxException>(() =>
            {
                var viewModel = loader.LoadViewModel(request, state, args);
            });
        }
    }
}
