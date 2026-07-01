namespace Nivaes.App.Cross
{
    [Obsolete("Json", true)]
    public interface ICrossJsonConverter : ICrossTextSerializer
    {
        T? DeserializeObject<T>(Stream stream);
    }
}
