namespace Nivaes.App.Cross.AppKitOS
{
    using System;

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
