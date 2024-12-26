using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nivaes.App.Cross
{
    public class ViewModelRequest<TViewModel> :
        IViewModelRequest
        where TViewModel : IViewModel
    {
        public ViewModelRequest()
        {
        }
    }
}
