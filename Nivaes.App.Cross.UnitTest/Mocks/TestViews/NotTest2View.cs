// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.UnitTest
{
    using MvvmCross.UnitTest.Mocks.TestViewModels;
    using MvvmCross.Views;
    using Nivaes.App.Cross.ViewModels;

    public class NotTest2View : IMvxView
    {
        public object? DataContext { get; set; }
        IMvxViewModel? IMvxView.ViewModel { get; set; }
        public Test2ViewModel? ViewModel { get; set; }
    }
}
