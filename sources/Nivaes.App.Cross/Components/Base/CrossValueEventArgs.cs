namespace Nivaes.App.Cross
{
    using System;

    [Obsolete()]
    public class CrossValueEventArgs<T>
        : EventArgs
    {
        public CrossValueEventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; private set; }
    }
}
