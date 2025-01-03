namespace Nivaes.App.Cross.Presenters
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;

    public struct KeyPresentation
    {
        public int Key { get; set; }
        public Type Value { get; set; }

        public static KeyPresentation New<TView, TPresentationType>()
           where TView : IView
           where TPresentationType : IViewPresentation
        {
            return new KeyPresentation { Key = typeof(TView).GetHashCode(), Value = typeof(TPresentationType) };
        }
    }

    public class KeyPresentationComparer : IComparer<KeyPresentation>
    {
        public int Compare(KeyPresentation x, KeyPresentation y)
        {
            return x.Key.CompareTo(y.Key);
        }
    }

    public class ViewPresentationsManager
    {
        private KeyPresentation[] mPresentations { get; }

        public ViewPresentationsManager()
        {
            mPresentations = new KeyPresentation[0];
        }

        public ViewPresentationsManager(KeyPresentation[] presentations)
        {
            mPresentations = presentations;
            var keyInstanceResolverValues = new Span<KeyPresentation>(mPresentations);
            keyInstanceResolverValues.Sort(new KeyPresentationComparer());
        }

        public bool TryGetValue<TView>([MaybeNullWhen(false)] out Type presentationType)
            where TView : IView
        {
            return TryGetValue(typeof(TView), out presentationType);
        }

        public bool TryGetValue(Type viewType, [MaybeNullWhen(false)] out Type presentationType)
        {
            var result = TryGetValue(viewType.GetHashCode(), out int position);

            if (result)
            {
                presentationType = mPresentations[position].Value;
                return true;
            }
            else
            {
                presentationType = default;
                return false;
            }
        }


       
    }
}
