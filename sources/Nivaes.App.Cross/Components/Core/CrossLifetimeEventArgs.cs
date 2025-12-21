namespace Nivaes.App.Cross
{
    public class CrossLifetimeEventArgs(CrossLifetimeEvent lifetimeEvent) 
        : EventArgs
    {
        public CrossLifetimeEvent LifetimeEvent { get; } = lifetimeEvent;
    }
}