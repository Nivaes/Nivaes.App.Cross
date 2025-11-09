namespace Nivaes.App.Cross.Droid
{
    using Android.Views;

    /// <summary>
    /// Used by Android presenters to check if they need to include shared element animations on navigation
    /// </summary>
    public interface ICrossAndroidSharedElements
    {
        /// <summary>
        /// Fetches views to add to the shared elements transition.
        /// </summary>
        /// <param name="attribute"></param>
        /// <param name="request">The <see cref="MvxBasePresentationAttribute"/> used by the view navigating to.</param>
        /// <returns>An <see cref="IDictionary{key, value}"/> containing the identifier key and view to animate with assigned transition name.</returns>
        IDictionary<string, View> FetchSharedElementsToAnimate(CrossBasePresentationAttribute attribute, ICrossViewModelRequest request);
    }
}
