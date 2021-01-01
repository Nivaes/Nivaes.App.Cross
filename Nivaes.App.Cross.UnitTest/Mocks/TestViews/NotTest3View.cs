// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.UnitTest
{
    using MvvmCross.UnitTest.Mocks.TestViewModels;
    using Nivaes.App.Cross.ViewModels;
    using MvvmCross.Views;

    [MvxViewFor(typeof(Test3ViewModel))]
    public class NotTest3View : IMvxView
    {
        public object? DataContext { get; set; }
        public IMvxViewModel? ViewModel { get; set; }
    }
}
