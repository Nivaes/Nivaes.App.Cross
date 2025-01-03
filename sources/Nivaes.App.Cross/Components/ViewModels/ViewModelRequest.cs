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
        public ViewModelRequest(TViewModel viewModel)
        {
            ViewModel = viewModel;
        }

        public TViewModel ViewModel { get;}

        IViewModel IViewModelRequest.ViewModel => ViewModel;
    }
}
