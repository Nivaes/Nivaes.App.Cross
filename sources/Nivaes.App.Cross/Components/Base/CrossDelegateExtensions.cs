namespace Nivaes.App.Cross
{
    using System;

    public static class CrossDelegateExtensions
    {
        public static void Raise(this EventHandler eventHandler, object sender)

        {
            eventHandler?.Invoke(sender, EventArgs.Empty);
        }

        public static void Raise<T>(this EventHandler<CrossValueEventArgs<T>> eventHandler, object sender, T value)
        {
            eventHandler?.Invoke(sender, new CrossValueEventArgs<T>(value));
        }
    }
}
