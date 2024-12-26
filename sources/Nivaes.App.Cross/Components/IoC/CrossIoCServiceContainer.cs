namespace Nivaes.App.Cross
{
    using Nivaes.IoC;

    public partial class CrossIoCServiceContainer : IoCServiceContainer, ICrossIoCServiceContainer
    {
        protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
        {
            bootstrapper.AddSingleton<INavigationService, NavigationService>();

        }
    }
}
