namespace MvvmCross.Platforms.Ios.Views.Base
{
    using System;
    using MvvmCross.Base;
    using Nivaes.App.Cross;

    public interface IMvxEventSourceViewController 
        : ICrossDisposeSource
    {
        event EventHandler ViewDidLoadCalled;

        event EventHandler ViewDidLayoutSubviewsCalled;

        event EventHandler<CrossValueEventArgs<bool>> ViewWillAppearCalled;

        event EventHandler<CrossValueEventArgs<bool>> ViewDidAppearCalled;

        event EventHandler<CrossValueEventArgs<bool>> ViewDidDisappearCalled;

        event EventHandler<CrossValueEventArgs<bool>> ViewWillDisappearCalled;
    }
}
