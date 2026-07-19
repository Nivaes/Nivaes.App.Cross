using Activity = AndroidX.AppCompat.App.AppCompatActivity;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;

namespace Nivaes.App.Cross.Droid
{
    public class PressenterActionContext
        : IPressenterActionContext
    {
        public ViewModelRequest? PendingDetailFragmentRequests { get; set; }
        public ViewModelRequest? PendingDefaultDetailRequests { get; set; }

        private readonly IMvxAndroidCurrentTopActivity _androidCurrentTopActivity;       

        public Activity CurrentActivity => (Activity)_androidCurrentTopActivity.Activity;

        public FragmentManager? CurrentFragmentManager
        {
            get
            {
                if (CurrentActivity.IsActivityDead())
                    return null;

                return CurrentActivity!.SupportFragmentManager;
            }
        }
        public PressenterActionContext(IMvxAndroidCurrentTopActivity androidCurrentTopActivity)
        {
            _androidCurrentTopActivity = androidCurrentTopActivity;
        }
    }
}
