namespace MvvmCross.Platforms.Mac.Views.Base
{
    using System;
    using MvvmCross.Base;
    using Nivaes.App.Cross;

    public interface IMvxEventSourceViewController 
        : ICrossDisposeSource
    {
        event EventHandler ViewDidLoadCalled;

        event EventHandler ViewDidLayoutCalled;

        event EventHandler ViewWillAppearCalled;

        event EventHandler ViewDidAppearCalled;

        event EventHandler ViewDidDisappearCalled;

        event EventHandler ViewWillDisappearCalled;
    }
}
