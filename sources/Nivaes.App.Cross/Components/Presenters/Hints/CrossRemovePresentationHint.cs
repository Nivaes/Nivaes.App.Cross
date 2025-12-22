namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using MvvmCross.ViewModels;

    public class CrossRemovePresentationHint
        : CrossPresentationHint
    {
        public CrossRemovePresentationHint(Type viewModelToRemove)
        {
            ViewModelToRemove = viewModelToRemove;
        }

        public CrossRemovePresentationHint(Type viewModelToRemove, CrossBundle body) : base(body)
        {
            ViewModelToRemove = viewModelToRemove;
        }

        public CrossRemovePresentationHint(Type viewModelToRemove, IDictionary<string, string> hints)
            : this(viewModelToRemove, new CrossBundle(hints))
        {
        }

        public Type ViewModelToRemove { get; }
    }
}
