using Android.Views;

namespace Nivaes.App.Cross;

public class CrossDroidVisibility : ICrossNativeVisibility
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
