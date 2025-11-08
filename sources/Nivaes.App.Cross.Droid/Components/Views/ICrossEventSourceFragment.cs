// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using Android.Content;
using Android.OS;
using MvvmCross.Base;

namespace Nivaes.App.Cross.Droid
{
    public interface ICrossEventSourceFragment : IMvxDisposeSource
    {
        //Created sate
        event EventHandler<CrossValueEventArgs<Context>> AttachCalled;

        event EventHandler<CrossValueEventArgs<Bundle>> CreateWillBeCalled;

        event EventHandler<CrossValueEventArgs<Bundle>> CreateCalled;

        event EventHandler<CrossValueEventArgs<CrossCreateViewParameters>> CreateViewCalled;

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
