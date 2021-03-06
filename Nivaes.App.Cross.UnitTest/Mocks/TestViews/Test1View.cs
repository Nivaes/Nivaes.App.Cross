﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.UnitTest.Mocks.TestViews
{
    using MvvmCross.Views;
    using Nivaes.App.Cross.ViewModels;

    public class Test1View : IMvxView
    {
        public object? DataContext { get; set; }
        public IMvxViewModel? ViewModel { get; set; }
    }
}
