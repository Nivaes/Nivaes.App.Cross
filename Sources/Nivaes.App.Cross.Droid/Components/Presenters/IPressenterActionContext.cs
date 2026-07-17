using Activity = AndroidX.AppCompat.App.AppCompatActivity;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;

namespace Nivaes.App.Cross.Droid
{
    public interface IPressenterActionContext
    {
        Activity CurrentActivity { get; }
        FragmentManager? CurrentFragmentManager { get; }

        CrossViewModelRequest? PendingDetailFragmentRequests { get; set; }
        CrossViewModelRequest? PendingDefaultDetailRequests { get; set; }
    }
}
