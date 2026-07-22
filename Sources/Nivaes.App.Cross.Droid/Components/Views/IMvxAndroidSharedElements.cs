using Android.Views;

namespace Nivaes.App.Cross.Droid
{
    /// <summary>
    /// Used by Android presenters to check if they need to include shared element animations on navigation
    /// </summary>
    public interface IMvxAndroidSharedElements
    {
        /// <summary>
        /// Fetches views to add to the shared elements transition.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="request">The <see cref="BasePresentationAttribute"/> used by the view navigating to.</param>
        /// <returns>An <see cref="IDictionary{key, value}"/> containing the identifier key and view to animate with assigned transition name.</returns>
        IDictionary<string, View> FetchSharedElementsToAnimate(BasePresentationAttribute attribute, IViewModelRequest request);
    }
}
