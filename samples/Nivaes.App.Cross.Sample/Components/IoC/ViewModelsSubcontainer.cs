using Nivaes.IoC;

namespace Nivaes.App.Cross.Sample;

// ToDo: Generar de forma automatica
public partial class ViewModelsSubcontainer 
    : IoCServiceContainer
{
    protected override void Bootstrap(IIoCServiceContainerBootstrapper bootstrapper)
    {
        bootstrapper.AddSingleton<RootViewModel>();
        //bootstrapper.AddSingleton<NewWindowViewModel>();
        //bootstrapper.AddSingleton<MainViewModel>();
        //bootstrapper.AddSingleton<BaseViewModel>();
        bootstrapper.AddSingleton<ChildViewModel>();
    }
}
