namespace Nivaes.App.Cross.Droid
{
    using System;
    using Android.App;
    using Android.Content;
    using Android.OS;

    public abstract class CrossBaseActivityAdapter
    {
        private readonly ICrossEventSourceActivity _eventSource;

        protected Activity Activity => _eventSource as Activity;

        protected CrossBaseActivityAdapter(ICrossEventSourceActivity eventSource)
        {
            _eventSource = eventSource;

            _eventSource.CreateCalled += EventSourceOnCreateCalled;
            _eventSource.CreateWillBeCalled += EventSourceOnCreateWillBeCalled;
            _eventSource.StartCalled += EventSourceOnStartCalled;
            _eventSource.RestartCalled += EventSourceOnRestartCalled;
            _eventSource.ResumeCalled += EventSourceOnResumeCalled;
            _eventSource.PauseCalled += EventSourceOnPauseCalled;
            _eventSource.StopCalled += EventSourceOnStopCalled;
            _eventSource.DestroyCalled += EventSourceOnDestroyCalled;
            _eventSource.DisposeCalled += EventSourceOnDisposeCalled;
            _eventSource.SaveInstanceStateCalled += EventSourceOnSaveInstanceStateCalled;
            _eventSource.NewIntentCalled += EventSourceOnNewIntentCalled;

            _eventSource.ActivityResultCalled += EventSourceOnActivityResultCalled;
            _eventSource.StartActivityForResultCalled += EventSourceOnStartActivityForResultCalled;
        }

        protected virtual void EventSourceOnSaveInstanceStateCalled(
            object sender, CrossValueEventArgs<Bundle> eventArgs)
        {
        }

        protected virtual void EventSourceOnCreateWillBeCalled(
            object sender, CrossValueEventArgs<Bundle> eventArgs)
        {
        }

        protected virtual void EventSourceOnStopCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnStartCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnStartActivityForResultCalled(
            object sender, CrossValueEventArgs<CrossStartActivityForResultParameters> eventArgs)
        {
        }

        protected virtual void EventSourceOnResumeCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnRestartCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnPauseCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnNewIntentCalled(object sender, CrossValueEventArgs<Intent> eventArgs)
        {
        }

        protected virtual void EventSourceOnDisposeCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnDestroyCalled(object sender, EventArgs eventArgs)
        {
        }

        protected virtual void EventSourceOnCreateCalled(object sender, CrossValueEventArgs<Bundle> eventArgs)
        {
        }

        protected virtual void EventSourceOnActivityResultCalled(
            object sender, CrossValueEventArgs<CrossActivityResultParameters> eventArgs)
        {
        }
    }
}
