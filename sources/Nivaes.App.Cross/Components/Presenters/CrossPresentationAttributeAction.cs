namespace Nivaes.App.Cross
{
    using System;
    using System.Threading.Tasks;
    using MvvmCross.ViewModels;
    using Nivaes.App.Cross;

    public class CrossPresentationAttributeAction
    {
        public Func<Type, ICrossPresentationAttribute, CrossViewModelRequest, Task<bool>>? ShowAction { get; set; }

        public Func<ICrossViewModel, ICrossPresentationAttribute, Task<bool>>? CloseAction { get; set; }
    }
}
