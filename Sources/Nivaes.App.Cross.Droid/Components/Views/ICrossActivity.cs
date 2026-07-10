using FragmentManager = AndroidX.Fragment.App.FragmentManager;

namespace Nivaes.App.Cross.Droid
{
    public interface ICrossActivity
    {
        FragmentManager SupportFragmentManager { get; }
    }
}
