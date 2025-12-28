using Nivaes.IoC;
namespace Nivaes.App.Cross.UIKitOS;

public partial class UIKitSubcontainer : IoCServiceContainer
{
    protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
    {
        //bootstrapper.AddSingleton<ICrossViewDispatcher, UIKitViewDispatcher>();

        //bootstrapper.AddSingleton<RootViewPresentation>();
    }
}
