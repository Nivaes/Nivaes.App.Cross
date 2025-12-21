namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using MvvmCross.ViewModels;

    public class CrossPopPresentationHint
        : CrossPresentationHint
    {
        public CrossPopPresentationHint(Type viewModelToPopTo, bool animated = false)
        {
            ViewModelToPopTo = viewModelToPopTo;
            Animated = animated;
        }

        public CrossPopPresentationHint(CrossBundle body, Type viewModelToPopTo, bool animated = true) : base(body)
        {
            ViewModelToPopTo = viewModelToPopTo;
            Animated = animated;
        }

        public CrossPopPresentationHint(IDictionary<string, string> hints, Type viewModelToPopTo, bool animated = true)
            : this(new CrossBundle(hints), viewModelToPopTo, animated)
        {
        }

        public Type ViewModelToPopTo { get; }

        public bool Animated { get; set; }
    }
}
