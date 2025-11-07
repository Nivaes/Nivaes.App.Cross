namespace Nivaes.App.Cross
{
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
