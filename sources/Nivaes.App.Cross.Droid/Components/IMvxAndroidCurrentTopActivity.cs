using static Android.App.Application;

namespace Nivaes.App.Cross.Droid
{
    public interface IMvxAndroidCurrentTopActivity : IActivityLifecycleCallbacks
    {
        Activity Activity { get; }
    }
}
