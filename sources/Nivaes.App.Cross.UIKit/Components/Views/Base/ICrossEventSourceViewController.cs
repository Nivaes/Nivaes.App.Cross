namespace Nivaes.App.Cross.UIKit
{
    using MvvmCross.Base;

    public interface ICrossEventSourceViewController : ICrossDisposeSource
    {
        event EventHandler ViewDidLoadCalled;

        event EventHandler ViewDidLayoutSubviewsCalled;

        event EventHandler<CrossValueEventArgs<bool>> ViewWillAppearCalled;

        event EventHandler<CrossValueEventArgs<bool>> ViewDidAppearCalled;

        event EventHandler<CrossValueEventArgs<bool>> ViewDidDisappearCalled;

        event EventHandler<CrossValueEventArgs<bool>> ViewWillDisappearCalled;
    }
}
