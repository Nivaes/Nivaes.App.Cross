namespace Nivaes.App.Cross
{
    [Obsolete("")]
    public class CrossStringDictionaryNavigationSerializer
        : ICrossNavigationSerializer
    {
        public ICrossTextSerializer Serializer { get; } = new CrossViewModelRequestCustomTextSerializer();
    }
}
