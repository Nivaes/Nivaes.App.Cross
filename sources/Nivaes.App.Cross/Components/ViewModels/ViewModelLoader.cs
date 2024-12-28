using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nivaes.IoC;

namespace Nivaes.App.Cross
{
    public static class ViewModelLoader
    {
        public static IViewModel LoadViewModel<TViewModel>()
             where TViewModel : IViewModel
        {
            var container = Singleton<CrossIoCServiceContainer>.Instance;
            var viewModel = container.Resolve<TViewModel>();

            return viewModel;
        }
    }
}
