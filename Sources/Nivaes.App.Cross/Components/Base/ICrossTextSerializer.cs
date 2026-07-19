namespace Nivaes.App.Cross
{
    [Obsolete("", true)]
    public interface ICrossTextSerializer
    {
        T? DeserializeObject<T>(string inputText);

        object? DeserializeObject(Type type, string inputText);

        string SerializeObject(object toSerialise);
    }
}
