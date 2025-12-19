namespace Nivaes.App.Cross
{
    using Nivaes.IoC;

    [Obsolete("Quitar IoC de Cross")]
    public partial class CrossIoCServiceContainer : IoCServiceContainer, ICrossIoCServiceContainer
    {
        protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
        {
            bootstrapper.AddSingleton<ICrossNavigationService, CrossNavigationService>();
        }
    }
}
