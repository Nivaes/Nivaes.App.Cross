using Nivaes.IoC;

namespace Nivaes.App.Cross;

public partial class CrossIoCServiceContainer : IoCServiceContainer, ICrossIoCServiceContainer
{
    protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
    {
        bootstrapper.AddSingleton<ICrossNavigationService, CrossNavigationService>();
    }
}
