namespace Nivaes.App.Cross
{
    [Obsolete("Quitar IoC de Cross")]
    public enum CrossSetupState
    {
        Uninitialized,
        InitializingPrimary,
        InitializedPrimary,
        InitializingSecondary,
        Initialized
    }
}
