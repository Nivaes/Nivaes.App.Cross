namespace Nivaes.App.Cross
{
    using System.Collections.Generic;

    public class CrossClosePresentationHint
        : CrossPresentationHint
    {
        public CrossClosePresentationHint(ICrossViewModel viewModelToClose) : base()
        {
            ViewModelToClose = viewModelToClose;
        }

        public CrossClosePresentationHint(ICrossViewModel viewModelToClose, CrossBundle body) : base(body)
        {
            ViewModelToClose = viewModelToClose;
        }

        public CrossClosePresentationHint(ICrossViewModel viewModelToClose, IDictionary<string, string> hints) : this(viewModelToClose, new CrossBundle(hints))
        {
        }

        public ICrossViewModel ViewModelToClose { get; private set; }
    }
}
