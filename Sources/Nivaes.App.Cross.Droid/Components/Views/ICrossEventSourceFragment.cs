using Android.Content;

namespace Nivaes.App.Cross.Droid
{
    public interface ICrossEventSourceFragment
        : ICrossDisposeSource
    {
        //Created sate
        event EventHandler<CrossValueEventArgs<Context>> AttachCalled;

        event EventHandler<CrossValueEventArgs<Bundle?>> CreateWillBeCalled;

        event EventHandler<CrossValueEventArgs<Bundle?>> CreateCalled;

        event EventHandler<CrossValueEventArgs<MvxCreateViewParameters>> CreateViewCalled;

        //Started state
        event EventHandler StartCalled;

        //Resumed state
        event EventHandler ResumeCalled;

        //Paused state
        event EventHandler PauseCalled;

        //Stopped state
        event EventHandler StopCalled;

        //Destroyed state
        event EventHandler DestroyViewCalled;

        event EventHandler DestroyCalled;

        event EventHandler DetachCalled;

        event EventHandler<CrossValueEventArgs<Bundle>> SaveInstanceStateCalled;
    }
}
