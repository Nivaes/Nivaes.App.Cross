namespace Nivaes.App.Cross
{
    public class CrossTargetChangedEventArgs(object? value) : EventArgs
    {
        public object? Value { get; } = value;
    }
}