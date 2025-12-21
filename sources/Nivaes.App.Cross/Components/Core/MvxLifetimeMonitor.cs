namespace Nivaes.App.Cross
{
    using System;
    using Nivaes.App.Cross;

    public abstract class MvxLifetimeMonitor : ICrossLifetime
    {
#pragma warning disable CA1030 // Use events where appropriate
        protected void FireLifetimeChange(CrossLifetimeEvent which)
#pragma warning restore CA1030 // Use events where appropriate
        {
            LifetimeChanged?.Invoke(this, new CrossLifetimeEventArgs(which));
        }

        public event EventHandler<CrossLifetimeEventArgs>? LifetimeChanged;
    }
}
