namespace Nivaes.App.Cross
{
    public class CrossStringDictionaryNavigationSerializer
        : ICrossNavigationSerializer
    {
        public ICrossTextSerializer Serializer { get; } = new CrossViewModelRequestCustomTextSerializer();
    }
}
