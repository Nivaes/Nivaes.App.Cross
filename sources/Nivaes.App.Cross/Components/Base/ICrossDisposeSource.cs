namespace Nivaes.App.Cross
{
    using System;

    [Obsolete("No se si es imprescindible")]
    public interface ICrossDisposeSource
    {
        event EventHandler? DisposeCalled;
    }
}
