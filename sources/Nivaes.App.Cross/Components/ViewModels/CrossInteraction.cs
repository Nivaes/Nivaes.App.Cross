namespace Nivaes.App.Cross
{
    using System;

    public class CrossInteraction : ICrossInteraction
    {
        public void Raise()
        {
            Requested?.Raise(this);
        }

        public event EventHandler? Requested;
    }

    public class MvxInteraction<T> : ICrossInteraction<T>
    {
        public void Raise(T request)
        {
            Requested?.Raise(this, request);
        }

        public event EventHandler<CrossValueEventArgs<T>>? Requested;
    }
}
