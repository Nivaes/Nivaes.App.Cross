namespace Nivaes.App.Cross
{
    using System;

    public interface ICrossDisposeSource
    {
        event EventHandler? DisposeCalled;
    }
}
