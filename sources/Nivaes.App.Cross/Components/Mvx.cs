using Nivaes.IoC;

namespace Nivaes.App.Cross;

[Obsolete("Quitar MvxIoC", true)]
public static class Mvx
{
    /// <summary>
    /// Returns a singleton instance of the default IoC Provider. If possible use dependency injection instead.
    /// </summary>
    [Obsolete("Hacerlo por inyección de dependencias", true)]
    public static IIoCResolver? IoCProvider => Singleton<CrossIoCServiceContainer>.Instance;
}
