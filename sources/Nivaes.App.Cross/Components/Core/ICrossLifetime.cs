namespace Nivaes.App.Cross
{
    using System;

    [Obsolete("Quitar IoC de Cross")]
    public interface ICrossLifetime
    {
        event EventHandler<CrossLifetimeEventArgs>? LifetimeChanged;
    }
}
