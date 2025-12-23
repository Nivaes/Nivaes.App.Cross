namespace Nivaes.App.Cross
{
    using System;

    public static class CrossDelegateExtensions
    {
        extension(EventHandler eventHandler)
        {
            public void Raise(object sender)

            {
                eventHandler?.Invoke(sender, EventArgs.Empty);
            }
        }

        extension<T>(EventHandler<CrossValueEventArgs<T>> eventHandler)
        {
            public void Raise(object sender, T value)
            {
                eventHandler?.Invoke(sender, new CrossValueEventArgs<T>(value));
            }
        }
    }
}