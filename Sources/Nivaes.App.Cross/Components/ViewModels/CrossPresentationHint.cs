namespace Nivaes.App.Cross
{
    public abstract class CrossPresentationHint
    {
        protected CrossPresentationHint(CrossBundle? body = default)
        {
            Body = body;
        }

        protected CrossPresentationHint(IDictionary<string, string> hints)
            : this(new CrossBundle(hints))
        {
        }

        public CrossBundle? Body { get; }
    }
}