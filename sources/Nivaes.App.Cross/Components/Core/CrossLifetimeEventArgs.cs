namespace Nivaes.App.Cross
{
    using System;

    [Obsolete("Quitar IoC de Cross")]
    public class CrossLifetimeEventArgs(CrossLifetimeEvent lifetimeEvent) 
        : EventArgs
    {
        public CrossLifetimeEvent LifetimeEvent { get; } = lifetimeEvent;
    }
}