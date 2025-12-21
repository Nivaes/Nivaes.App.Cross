namespace Nivaes.App.Cross
{
    using System;
    using MvvmCross.Base;

    public interface ICrossInteraction
    {
        event EventHandler? Requested;
    }

    public interface ICrossInteraction<T>
    {
        event EventHandler<CrossValueEventArgs<T>>? Requested;
    }
}
