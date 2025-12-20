namespace MvvmCross.Presenters.Hints
{
    using System;
    using System.Collections.Generic;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class MvxRemovePresentationHint
        : MvxPresentationHint
    {
        public MvxRemovePresentationHint(Type viewModelToRemove)
        {
            ViewModelToRemove = viewModelToRemove;
        }

        public MvxRemovePresentationHint(Type viewModelToRemove, CrossBundle body) : base(body)
        {
            ViewModelToRemove = viewModelToRemove;
        }

        public MvxRemovePresentationHint(Type viewModelToRemove, IDictionary<string, string> hints)
            : this(viewModelToRemove, new CrossBundle(hints))
        {
        }

        public Type ViewModelToRemove { get; }
    }
}
