namespace Nivaes.App.Cross
{
    using System;

    [Obsolete("Quitar IoC de Cross")]
    public abstract class CrossLifetimeMonitor 
        : ICrossLifetime
    {
        protected void FireLifetimeChange(CrossLifetimeEvent which)
        {
            LifetimeChanged?.Invoke(this, new CrossLifetimeEventArgs(which));
        }

        public event EventHandler<CrossLifetimeEventArgs>? LifetimeChanged;
    }
}
