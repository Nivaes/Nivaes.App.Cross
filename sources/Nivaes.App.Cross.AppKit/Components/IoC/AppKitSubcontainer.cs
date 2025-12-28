namespace Nivaes.App.Cross.AppKitOS
{
    using Nivaes.IoC;

    public partial class AppKitSubcontainer : IoCServiceContainer
    {
        protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
        {
            //bootstrapper.AddSingleton<ICrossViewDispatcher, AppKitViewDispatcher>();

            //bootstrapper.AddSingleton<PageViewPresentation>();
            //bootstrapper.AddSingleton<NewWindowViewPresentation>();
        }
    }
}
