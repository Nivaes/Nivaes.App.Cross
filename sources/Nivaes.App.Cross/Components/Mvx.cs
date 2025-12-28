namespace MvvmCross
{
    using MvvmCross.IoC;
    using Nivaes.App.Cross;

    [Obsolete("Quitar MvxIoC")]
    public static class Mvx
    {
        /// <summary>
        /// Returns a singleton instance of the default IoC Provider. If possible use dependency injection instead.
        /// </summary>
        [Obsolete("Quitar MvxIoC")]
        public static IMvxIoCProvider? IoCProvider => CrossSingleton<IMvxIoCProvider>.Instance;
    }
}
