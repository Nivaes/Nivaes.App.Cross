namespace Nivaes.App.Cross.Sample
{
    using Nivaes.IoC;

    public partial class ViewModelsSubcontainer 
        : IoCServiceContainer
    {
        protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
        {
            bootstrapper.AddSingleton<RootViewModel>();
            bootstrapper.AddSingleton<NewWindowViewModel>();
            bootstrapper.AddSingleton<FormViewModel>();
            bootstrapper.AddSingleton<SubFormViewModel>();
            bootstrapper.AddSingleton<SubSubFormViewModel>();
        }
    }
}
