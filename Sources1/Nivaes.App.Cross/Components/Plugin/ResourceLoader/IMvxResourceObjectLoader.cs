namespace Nivaes.App.Cross;

public interface IMvxResourceObjectLoader<out T>
    where T : IMvxResourceObject
{
    T Load(string namespaceKey, string typeKey, string entryKey);
}
