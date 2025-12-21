namespace MvvmCross.Presenters.Hints
{
    using System.Collections.Generic;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class MvxPopToRootPresentationHint
        : CrossPresentationHint
    {
        public MvxPopToRootPresentationHint(bool animated = true)
        {
            Animated = animated;
        }

        public MvxPopToRootPresentationHint(CrossBundle body, bool animated = true) : base(body)
        {
            Animated = animated;
        }

        public MvxPopToRootPresentationHint(IDictionary<string, string> hints, bool animated = true)
            : this(new CrossBundle(hints), animated)
        {
        }

        public bool Animated { get; set; }
    }
#nullable restore
}
