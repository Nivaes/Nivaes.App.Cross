namespace Nivaes.App.Cross
{
    using System;
    using System.Threading.Tasks;

    public class CrossPresentationAttributeAction
    {
        public Func<Type, ICrossPresentationAttribute, CrossViewModelRequest, Task<bool>>? ShowAction { get; set; }

        public Func<ICrossViewModel, ICrossPresentationAttribute, Task<bool>>? CloseAction { get; set; }
    }
}
