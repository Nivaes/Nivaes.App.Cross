namespace Nivaes.App.Cross
{
    using System;

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
