using Nivaes.IoC;

namespace Nivaes.App.Cross;

[Obsolete("Quitar MvxIoC")]
public static class Mvx
{
    /// <summary>
    /// Returns a singleton instance of the default IoC Provider. If possible use dependency injection instead.
    /// </summary>
    [Obsolete("Hacerlo por inyección de dependencias")]
    public static IIoCResolver? IoCProvider => Singleton<CrossIoCServiceContainer>.Instance;
}
