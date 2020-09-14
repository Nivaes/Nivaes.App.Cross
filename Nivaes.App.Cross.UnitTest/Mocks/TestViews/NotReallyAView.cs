﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace MvvmCross.UnitTest.Mocks.TestViews
{
    using MvvmCross.ViewModels;

    public class NotReallyAView
    {
        public object? DataContext { get; set; }
        public IMvxViewModel? ViewModel { get; set; }
    }
}
