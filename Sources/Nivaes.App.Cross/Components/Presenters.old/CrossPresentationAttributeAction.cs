using System.Diagnostics;

namespace Nivaes.App.Cross
{
    [Obsolete("", true)]
    public class CrossPresentationAttributeAction
    {
        public Func<Type, IPresentationAttribute, CrossViewModelRequest, Task<bool>>? ShowAction 
        { 
            get; 
            set; 
        }

        public Func<ICrossViewModel, IPresentationAttribute, Task<bool>>? CloseAction 
        { 
            get; 
            set; 
        }
    }
}
