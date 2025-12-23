namespace Nivaes.App.Cross.Droid
{
    using System;
    using Android.Content;
    using MvvmCross.Platforms.Android.Views.Base;
    using Nivaes.App.Cross;

    public interface IMvxEventSourceActivity 
        : ICrossDisposeSource
    {
        event EventHandler<CrossValueEventArgs<Bundle>> CreateWillBeCalled;

        event EventHandler<CrossValueEventArgs<Bundle>> CreateCalled;

        event EventHandler DestroyCalled;

        event EventHandler<CrossValueEventArgs<Intent>> NewIntentCalled;

        event EventHandler ResumeCalled;

        event EventHandler PauseCalled;

        event EventHandler StartCalled;

        event EventHandler RestartCalled;

        event EventHandler StopCalled;

        event EventHandler<CrossValueEventArgs<Bundle>> SaveInstanceStateCalled;

        event EventHandler<CrossValueEventArgs<MvxStartActivityForResultParameters>> StartActivityForResultCalled;

        event EventHandler<CrossValueEventArgs<MvxActivityResultParameters>> ActivityResultCalled;
    }
}
