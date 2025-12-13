namespace Nivaes.App.Cross
{
    using System.IO;

    public interface ICrossJsonConverter : ICrossTextSerializer
    {
        T? DeserializeObject<T>(Stream stream);
    }
}
