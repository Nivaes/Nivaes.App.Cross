using Android.Views;

namespace MvvmCross.Plugin.Visibility.Platforms.Android
{
    using Nivaes.App.Cross;

    [Preserve(AllMembers = true)]
    public class MvxDroidVisibility : ICrossNativeVisibility
    {
        public object ToNative(CrossVisibility visibility)
        {
            switch (visibility)
            {
                case CrossVisibility.Collapsed:
                    return ViewStates.Gone;
                case CrossVisibility.Hidden:
                    return ViewStates.Invisible;
                default:
                    return ViewStates.Visible;
            }
        }
    }
}
