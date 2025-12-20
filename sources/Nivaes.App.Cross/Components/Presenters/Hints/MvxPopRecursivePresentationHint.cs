namespace MvvmCross.Presenters.Hints
{
    using System;
    using System.Collections.Generic;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class MvxPopRecursivePresentationHint
        : MvxPresentationHint
    {
        public MvxPopRecursivePresentationHint(int levelsDeep, bool animated = false)
        {
            LevelsDeep = levelsDeep;
            Animated = animated;
        }

        public MvxPopRecursivePresentationHint(CrossBundle body, int levelsDeep, bool animated = true) : base(body)
        {
            LevelsDeep = levelsDeep;
            Animated = animated;
        }

        public MvxPopRecursivePresentationHint(IDictionary<string, string> hints, int levelsDeep, bool animated = true)
            : this(new CrossBundle(hints), levelsDeep, animated)
        {
        }

        public int LevelsDeep { get; }

        public bool Animated { get; set; }
    }
}
