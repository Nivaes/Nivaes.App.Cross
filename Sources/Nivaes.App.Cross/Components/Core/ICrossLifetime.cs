namespace Nivaes.App.Cross
{
    using System;

    public interface ICrossLifetime
    {
        event EventHandler<CrossLifetimeEventArgs>? LifetimeChanged;
    }
}
