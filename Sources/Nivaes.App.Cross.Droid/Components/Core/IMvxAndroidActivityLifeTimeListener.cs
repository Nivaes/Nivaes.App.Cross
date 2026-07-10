using System;
using Nivaes.App.Cross;

namespace Nivaes.App.Cross.Droid
{
    public interface IMvxAndroidActivityLifetimeListener
        : ICrossLifetime
    {
        void OnCreate(Activity activity, Bundle? eventArgs);

        void OnStart(Activity activity);

        void OnRestart(Activity activity);

        void OnResume(Activity activity);

        void OnPause(Activity activity);

        void OnStop(Activity activity);

        void OnDestroy(Activity activity);

        void OnViewNewIntent(Activity activity);

        void OnSaveInstanceState(Activity activity, Bundle? eventArgs);

        event EventHandler<MvxActivityEventArgs> ActivityChanged;
    }
}
