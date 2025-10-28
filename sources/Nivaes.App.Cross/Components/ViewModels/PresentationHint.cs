namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using Nivaes.App.Cross.Components.ViewModels;

    // ToDo: Comprobar que se usa para algo.
    internal class NivCrossPresentationHint
    {
        protected NivCrossPresentationHint(CrossBundle? body = default)
        {
            Body = body;
        }

        protected NivCrossPresentationHint(IDictionary<string, string> hints)
            : this(new CrossBundle(hints))
        {
        }

        public CrossBundle Body { get; private set; }
    }
}
