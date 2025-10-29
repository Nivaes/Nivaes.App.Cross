namespace Nivaes.App.Cross
{
    using System.Collections.Generic;

    // ToDo: Comprobar que se usa para algo.
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
