using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nivaes.App.Cross
{
    public record ViewModelRequest<TViewModel> :
        IViewModelRequest
        where TViewModel : IViewModel
    {
        public ViewModelRequest()
        {
        }

        public IViewModel ViewModel { get; set; }
    }
}
