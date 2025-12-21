namespace MvvmCross.Presenters.Hints
{
    using System.Collections.Generic;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class MvxClosePresentationHint
        : CrossPresentationHint
    {
        public MvxClosePresentationHint(ICrossViewModel viewModelToClose)
        {
            ViewModelToClose = viewModelToClose;
        }

        public MvxClosePresentationHint(ICrossViewModel viewModelToClose, CrossBundle body) : base(body)
        {
            ViewModelToClose = viewModelToClose;
        }

        public MvxClosePresentationHint(ICrossViewModel viewModelToClose, IDictionary<string, string> hints) : this(viewModelToClose, new CrossBundle(hints))
        {
        }

        public ICrossViewModel ViewModelToClose { get; }
    }
}