namespace Nivaes.App.Cross
{
    using System;
    using System.Diagnostics.CodeAnalysis;

    public sealed class ViewsManager : KeyContainerManager<Type>
    {
        public static KeyStoreItem New<TViewModel, TView>()
            where TViewModel : IViewModel
            where TView : IView
        {
            return new KeyStoreItem { Key = typeof(TViewModel).GetHashCode(), Value = typeof(TView) };
        }

        public ViewsManager()
        {
        }

        public ViewsManager(KeyStoreItem[] presentations)
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
