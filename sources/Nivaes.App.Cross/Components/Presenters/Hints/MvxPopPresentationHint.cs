namespace MvvmCross.Presenters.Hints
{
    using System;
    using System.Collections.Generic;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class MvxPopPresentationHint
        : CrossPresentationHint
    {
        public MvxPopPresentationHint(Type viewModelToPopTo, bool animated = false)
        {
            ViewModelToPopTo = viewModelToPopTo;
            Animated = animated;
        }

        public MvxPopPresentationHint(CrossBundle body, Type viewModelToPopTo, bool animated = true) : base(body)
        {
            ViewModelToPopTo = viewModelToPopTo;
            Animated = animated;
        }

        public MvxPopPresentationHint(IDictionary<string, string> hints, Type viewModelToPopTo, bool animated = true)
            : this(new CrossBundle(hints), viewModelToPopTo, animated)
        {
        }

        public Type ViewModelToPopTo { get; }

        public bool Animated { get; set; }
    }
}
