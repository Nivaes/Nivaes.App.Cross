namespace Nivaes.App.Cross
{
    [Obsolete("", true)]
    public interface ICrossJsonConverter : ICrossTextSerializer
    {
        T? DeserializeObject<T>(Stream stream);
    }
}
