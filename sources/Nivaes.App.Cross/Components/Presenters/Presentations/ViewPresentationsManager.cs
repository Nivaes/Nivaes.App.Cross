namespace Nivaes.App.Cross.Presenters
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class ViewPresentationsManager
    {
        private IDictionary<int, Type> mPresentations { get; } = new Dictionary<int, Type>();

        public ViewPresentationsManager()
        {
        }

        public void AddPresentation<TView, TPresentationType>()
            where TView : IView
            where TPresentationType : IViewPresentation
        {
            mPresentations.Add(typeof(TView).GetHashCode(), typeof(TPresentationType));
        }

        public bool TryGetValue<TView>([MaybeNullWhen(false)] out Type presentationType)
            where TView : IView
        {
            return TryGetValue(typeof(TView), out presentationType);
        }

        public bool TryGetValue(Type viewType, [MaybeNullWhen(false)] out Type presentationType)
        {
            return TryGetValue(viewType.GetHashCode(), out presentationType);
        }

        private bool TryGetValue(int viewTypeHash, [MaybeNullWhen(false)] out Type presentationType)
        {
            return mPresentations.TryGetValue(viewTypeHash, out presentationType);
        }
    }
}
