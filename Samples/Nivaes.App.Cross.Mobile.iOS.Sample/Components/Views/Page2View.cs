﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Mobile.iOS.Sample
{
    using System;
    using MvvmCross.Platforms.Ios.Presenters.Attributes;
    using MvvmCross.Platforms.Ios.Views;
    using Nivaes.App.Mobile.Sample;

    [MvxFromStoryboard("Main")]
    [MvxPagePresentation(WrapInNavigationController = false)]
    public partial class Page2View : MvxViewController<Page2ViewModel>
    {
        public Page2View(IntPtr handle) : base(handle)
        {
        }
    }
}
