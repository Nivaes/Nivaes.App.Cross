namespace Nivaes.App.Cross
{
    using System;

    public interface ICrossInteraction
    {
        event EventHandler? Requested;
    }

    public interface ICrossInteraction<T>
    {
        event EventHandler<CrossValueEventArgs<T>>? Requested;
    }
}
