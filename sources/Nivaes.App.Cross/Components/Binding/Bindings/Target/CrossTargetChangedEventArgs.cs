namespace Nivaes.App.Cross
{
    using System;

    public class CrossTargetChangedEventArgs(object? value) : EventArgs
    {
        public object? Value { get; } = value;
    }
}