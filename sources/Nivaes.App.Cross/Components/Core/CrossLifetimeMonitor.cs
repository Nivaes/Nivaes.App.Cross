namespace Nivaes.App.Cross
{
    using System;
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
