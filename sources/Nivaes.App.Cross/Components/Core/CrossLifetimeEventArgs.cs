namespace Nivaes.App.Cross
{
    using System;

    public class CrossLifetimeEventArgs(CrossLifetimeEvent lifetimeEvent) 
        : EventArgs
    {
        public CrossLifetimeEvent LifetimeEvent { get; } = lifetimeEvent;
    }
}