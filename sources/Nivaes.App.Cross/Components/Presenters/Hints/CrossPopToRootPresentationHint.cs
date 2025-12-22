namespace Nivaes.App.Cross
{
    using System.Collections.Generic;
    using MvvmCross.ViewModels;

    public class CrossPopToRootPresentationHint
        : CrossPresentationHint
    {
        public CrossPopToRootPresentationHint(bool animated = true)
        {
            Animated = animated;
        }

        public CrossPopToRootPresentationHint(CrossBundle body, bool animated = true) : base(body)
        {
            Animated = animated;
        }

        public CrossPopToRootPresentationHint(IDictionary<string, string> hints, bool animated = true)
            : this(new CrossBundle(hints), animated)
        {
        }

        public bool Animated { get; set; }
    }
#nullable restore
}
