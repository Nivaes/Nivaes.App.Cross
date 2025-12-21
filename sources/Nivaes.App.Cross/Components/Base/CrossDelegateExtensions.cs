namespace Nivaes.App.Cross
{
    using System;

    public static class CrossDelegateExtensions
    {
#pragma warning disable CA1030 // Use events where appropriate
        public static void Raise(this EventHandler eventHandler, object sender)

        {
            eventHandler?.Invoke(sender, EventArgs.Empty);
        }

        public static void Raise<T>(this EventHandler<CrossValueEventArgs<T>> eventHandler, object sender, T value)
        {
            eventHandler?.Invoke(sender, new CrossValueEventArgs<T>(value));
        }
#pragma warning restore CA1030 // Use events where appropriate
    }
}