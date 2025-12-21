namespace MvvmCross.ViewModels
{
    using Nivaes.App.Cross;

    public class MvxStringDictionaryNavigationSerializer
        : ICrossNavigationSerializer
    {
        public ICrossTextSerializer Serializer { get; } = new CrossViewModelRequestCustomTextSerializer();
    }
}
