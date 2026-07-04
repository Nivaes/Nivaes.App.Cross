namespace Nivaes.App.Cross.UIKitLib
{
    using System;

    public interface IMvxEventSourceViewController
        : ICrossDisposeSource
    {
        event EventHandler? ViewDidLoadCalled;

        event EventHandler? ViewDidLayoutSubviewsCalled;

        event EventHandler<CrossValueEventArgs<bool>>? ViewWillAppearCalled;

        event EventHandler<CrossValueEventArgs<bool>>? ViewDidAppearCalled;

        event EventHandler<CrossValueEventArgs<bool>>? ViewDidDisappearCalled;

        event EventHandler<CrossValueEventArgs<bool>>? ViewWillDisappearCalled;
    }
}
