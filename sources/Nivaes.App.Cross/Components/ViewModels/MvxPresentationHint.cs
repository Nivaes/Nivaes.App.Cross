namespace MvvmCross.ViewModels
{
    using System.Collections.Generic;
    using Nivaes.App.Cross;

    public abstract class MvxPresentationHint
    {
        protected MvxPresentationHint(CrossBundle? body = default)
        {
            Body = body;
        }

        protected MvxPresentationHint(IDictionary<string, string> hints)
            : this(new CrossBundle(hints))
        {
        }

        public CrossBundle? Body { get; }
    }
}