namespace MvvmCross.Presenters.Hints
{
    using System.Collections.Generic;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class MvxClosePresentationHint
        : MvxPresentationHint
    {
        public MvxClosePresentationHint(ICrossViewModel viewModelToClose)
        {
            ViewModelToClose = viewModelToClose;
        }

        public MvxClosePresentationHint(ICrossViewModel viewModelToClose, MvxBundle body) : base(body)
        {
            ViewModelToClose = viewModelToClose;
        }

        public MvxClosePresentationHint(ICrossViewModel viewModelToClose, IDictionary<string, string> hints) : this(viewModelToClose, new MvxBundle(hints))
        {
        }

        public ICrossViewModel ViewModelToClose { get; }
    }
}