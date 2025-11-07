namespace Nivaes.App.Cross
{
    public interface ICrossTargetBindingFactoryRegistry : ICrossTargetBindingFactory
    {
        void RegisterFactory(ICrossPluginTargetBindingFactory factory);
    }
}
