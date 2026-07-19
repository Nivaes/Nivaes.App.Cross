using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace Nivaes.App.Cross
{
    [Obsolete("", true)]
    public class CrossViewModelInstanceRequest(Type viewModelType)
            : ViewModelRequest(viewModelType)
    {
        public CrossViewModelInstanceRequest(ICrossViewModel viewModelInstance)
            : this(viewModelInstance.GetType())
        {
            ViewModelInstance = viewModelInstance;
        }

        public ICrossViewModel? ViewModelInstance 
        { 
            get; 
            set; 
        }
    }
}
