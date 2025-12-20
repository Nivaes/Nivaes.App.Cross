namespace MvvmCross.Presenters
{
    using System;
    using System.Threading.Tasks;
    using MvvmCross.Presenters.Attributes;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class MvxPresentationAttributeAction
    {
        public Func<Type, IMvxPresentationAttribute, MvxViewModelRequest, Task<bool>>? ShowAction { get; set; }

        public Func<ICrossViewModel, IMvxPresentationAttribute, Task<bool>>? CloseAction { get; set; }
    }
}
