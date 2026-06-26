namespace Nivaes.App.Cross.Droid
{
    using Nivaes.IoC;

    [Obsolete("", true)]
    public partial class DroidIoCServiceContainer
        : IoCServiceContainer
    {
        protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
        {
            //bootstrapper.AddSingleton<ICrossViewDispatcher, CrossDroidViewDispatcher>();

            //bootstrapper.AddSingleton<CrossActivityViewPresentation>();
        }
    }
}
