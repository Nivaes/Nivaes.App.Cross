namespace Nivaes.App.Cross
{
    [Obsolete("Quitar IoC de Cross")]
    public enum CrossLifetimeEvent
    {
        Launching,
        ActivatedFromMemory,
        ActivatedFromDisk,
        Deactivated,
        Closing
    }
}