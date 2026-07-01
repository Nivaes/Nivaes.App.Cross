namespace Nivaes.App.Cross
{
    using System.Diagnostics.CodeAnalysis;

    public interface ICrossTextSerializer
    {
        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        T? DeserializeObject<T>(string inputText);

        [RequiresUnreferencedCode("This method uses reflection which may not be preserved during trimming")]
        object? DeserializeObject(Type type, string inputText);

        string SerializeObject(object toSerialise);
    }
}
