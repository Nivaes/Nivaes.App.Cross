// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Mobile.iOS.Sample
{
    using System;
    using MvvmCross.Platforms.Ios.Views;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Cross.Sample;

    [MvxFromStoryboard("PagesRootView")]
    [MvxPagePresentation(WrapInNavigationController = false)]
    public partial class Page3View
        : MvxViewController<Page3ViewModel>
    {
        public Page3View(IntPtr handle)
            : base(handle)
        {
        }
    }
}
