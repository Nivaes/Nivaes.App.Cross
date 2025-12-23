namespace Nivaes.App.Cross
{
    using System.Collections.Generic;

    public class CrossPopRecursivePresentationHint
        : CrossPresentationHint
    {
        public CrossPopRecursivePresentationHint(int levelsDeep, bool animated = false)
        {
            LevelsDeep = levelsDeep;
            Animated = animated;
        }

        public CrossPopRecursivePresentationHint(CrossBundle body, int levelsDeep, bool animated = true) : base(body)
        {
            LevelsDeep = levelsDeep;
            Animated = animated;
        }

        public CrossPopRecursivePresentationHint(IDictionary<string, string> hints, int levelsDeep, bool animated = true)
            : this(new CrossBundle(hints), levelsDeep, animated)
        {
        }

        public int LevelsDeep { get; }

        public bool Animated { get; set; }
    }
}
