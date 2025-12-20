namespace Nivaes.App.Cross
{
    using System.Collections.Generic;

    public class CrossPresentationHint
    {
        protected CrossPresentationHint(CrossBundle? body = default)
        {
            Body = body;
        }

        protected CrossPresentationHint(IDictionary<string, string> hints)
            : this(new CrossBundle(hints))
        {
        }

        public CrossBundle? Body { get; private set; }
    }
}
