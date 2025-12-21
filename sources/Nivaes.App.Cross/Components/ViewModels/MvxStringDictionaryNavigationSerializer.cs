namespace MvvmCross.ViewModels
{
    using MvvmCross.Base;
    using MvvmCross.Core.Parse.StringDictionary;
    using Nivaes.App.Cross;

    public class MvxStringDictionaryNavigationSerializer
        : IMvxNavigationSerializer
    {
        public IMvxTextSerializer Serializer { get; } = new CrossViewModelRequestCustomTextSerializer();
    }
}
