namespace Nivaes.App.Cross
{
    public abstract class CrossLifetimeMonitor : ICrossLifetime
    {
        protected void FireLifetimeChange(CrossLifetimeEvent which)
        {
            LifetimeChanged?.Invoke(this, new CrossLifetimeEventArgs(which));
        }

        public event EventHandler<CrossLifetimeEventArgs>? LifetimeChanged;
    }
}
