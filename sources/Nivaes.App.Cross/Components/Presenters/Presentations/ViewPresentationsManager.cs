namespace Nivaes.App.Cross.Presenters
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public class ViewPresentationsManager
    {
        private IDictionary<int, IViewPresentation> mPresentations { get; } = new Dictionary<int, IViewPresentation>();

        public ViewPresentationsManager()
        {
        }

        public void AddPresentation<TView>(IViewPresentation presentation)
            where TView : IView
        {
            mPresentations.Add(typeof(TView).GetHashCode(), presentation);
        }

        public bool TryGetValue<TView>([MaybeNullWhen(false)] out IViewPresentation presentation)
            where TView : IView
        {
            return mPresentations.TryGetValue(typeof(TView).GetHashCode(), out presentation);
        }
    }
}
