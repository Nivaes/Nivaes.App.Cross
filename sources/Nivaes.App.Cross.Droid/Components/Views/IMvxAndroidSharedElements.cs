using Android.Views;

namespace MvvmCross.Platforms.Android.Views
{
    using System.Collections.Generic;
    using MvvmCross.Presenters.Attributes;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

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
        IDictionary<string, View> FetchSharedElementsToAnimate(MvxBasePresentationAttribute attribute, CrossViewModelRequest request);
    }
}
