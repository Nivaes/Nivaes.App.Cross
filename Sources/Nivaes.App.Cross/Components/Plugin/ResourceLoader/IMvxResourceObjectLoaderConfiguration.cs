namespace Nivaes.App.Cross;

public interface IMvxResourceObjectLoaderConfiguration<T>
    where T : IMvxResourceObject
{
    void SetRootLocation(string location);

    void SetRootLocation(string namespaceKey, string typeKey, string location);
}
