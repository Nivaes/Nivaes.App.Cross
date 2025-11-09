namespace Nivaes.App.Cross.UIKit
{
    using Nivaes.IoC;

    public partial class UIKitSubcontainer : IoCServiceContainer
    {
        protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
        {
            bootstrapper.AddSingleton<ICrossViewDispatcher, UIKitViewDispatcher>();

            bootstrapper.AddSingleton<RootViewPresentation>();
        }
    }
}
