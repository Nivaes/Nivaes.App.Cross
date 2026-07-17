using Android.Content;
using Android.OS;
using Android.Util;
using Microsoft.Extensions.Logging;
using Activity = AndroidX.AppCompat.App.AppCompatActivity;
using DialogFragment = AndroidX.Fragment.App.DialogFragment;
using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;
using FragmentTransaction = AndroidX.Fragment.App.FragmentTransaction;

namespace Nivaes.App.Cross.Droid
{
    internal interface IDetailPresentationAction
    {
        Fragment? FindFragmentHost(int fragmentContentId, FragmentManager fragmentManager)
        {
            foreach (var fragment in fragmentManager.Fragments)
            {
                if (fragment.View!.FindViewById(fragmentContentId) != null)
                {
                    return fragment;
                }
            }
            return null;
        }
    }
}
