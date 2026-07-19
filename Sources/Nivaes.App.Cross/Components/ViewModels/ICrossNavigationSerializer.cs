namespace Nivaes.App.Cross
{
    [Obsolete("")]
    public interface ICrossNavigationSerializer
    {
        ICrossTextSerializer Serializer { get; }
    }
}
