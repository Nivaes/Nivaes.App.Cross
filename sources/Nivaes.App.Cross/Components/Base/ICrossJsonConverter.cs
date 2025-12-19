namespace Nivaes.App.Cross
{
    using System.IO;

    [Obsolete()]
    public interface ICrossJsonConverter : ICrossTextSerializer
    {
        T? DeserializeObject<T>(Stream stream);
    }
}
