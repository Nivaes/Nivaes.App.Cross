namespace Nivaes.App.Cross.Presenters
{
    using System.Diagnostics.CodeAnalysis;

    public sealed class CrossViewPresentationsManager : KeyContainerManager<Type>
    {
        public static KeyStoreItem New<TView, TPresentationType>()
            where TView : ICrossView<>
            where TPresentationType : ICrossViewPresentation
        {
            return new KeyStoreItem { Key = typeof(TView).GetHashCode(), Value = typeof(TPresentationType) };
        }

        public CrossViewPresentationsManager()
        {
        }

        public CrossViewPresentationsManager(KeyStoreItem[] presentations)
            : base(presentations)
        {
        }

        public bool TryGetValue<TView>([MaybeNullWhen(false)] out Type presentationType)
        {
            return TryGetValue(typeof(TView), out presentationType);
        }

        public bool TryGetValue(Type viewType, [MaybeNullWhen(false)] out Type presentationType)
        {
            return TryGetValue(viewType.GetHashCode(), out presentationType);
        }
    }
}
