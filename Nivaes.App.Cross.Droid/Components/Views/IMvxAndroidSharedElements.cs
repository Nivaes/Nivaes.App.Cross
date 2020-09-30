// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using Android.Views;

namespace MvvmCross.Platforms.Android.Views
{
    using System.Collections.Generic;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross.Presenters;

    /// <summary>
    /// Used by Android presenters to check if they need to include shared element animations on navigation
    /// </summary>
    public interface IMvxAndroidSharedElements
    {
        /// <summary>
        /// Fetches views to add to the shared elements transition.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="request">The <see cref="MvxBasePresentationAttribute"/> used by the view navigating to.</param>
        /// <returns>An <see cref="IDictionary{key, value}"/> containing the identifier key and view to animate with assigned transition name.</returns>
        IDictionary<string, View> FetchSharedElementsToAnimate(MvxBasePresentationAttribute attribute, MvxViewModelRequest request);
    }
}
