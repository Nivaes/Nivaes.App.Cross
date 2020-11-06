// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

namespace Nivaes.App.Cross.Mobile.Droid.Sample
{
    using Android.Runtime;
    using Nivaes.App.Cross.Presenters;
    using Nivaes.App.Mobile.Sample;

    [MvxFragmentPresentation(typeof(SplitRootViewModel), Resource.Id.split_content_frame, AddToBackStack = true)]
    [Register(nameof(SplitDetailNavView))]
    public class SplitDetailNavView
        : BaseSplitDetailView<SplitDetailNavViewModel>
    {
        protected override int FragmentLayoutId => Resource.Layout.SplitDetailNavView;
    }
}
