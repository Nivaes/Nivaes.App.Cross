namespace Nivaes.App.Cross.Droid
{
    using Nivaes.IoC;

    public partial class DroidSubcontainer : IoCServiceContainer
    {
        protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
        {
            bootstrapper.AddSingleton<IViewDispatcher, DroidViewDispatcher>();

            //bootstrapper.AddSingleton<WinUIPageViewPresentation>();
            //bootstrapper.AddSingleton<WinUINewWindowViewPresentation>();
        }
    }
}
