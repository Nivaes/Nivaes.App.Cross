namespace Nivaes.App.Cross
{
    public interface ICrossJsonConverter : ICrossTextSerializer
    {
        T? DeserializeObject<T>(Stream stream);
    }
}
