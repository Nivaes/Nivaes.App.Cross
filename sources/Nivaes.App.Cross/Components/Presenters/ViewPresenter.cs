using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nivaes.App.Cross.Presenters
{
    public abstract class ViewPresenter : IViewPresenter
    {
        protected ViewPresenter()
        {
        }

        public abstract Task<bool> Show(IViewModelRequest request);

        public abstract Task<bool> Close(IViewModel request);
    }
}
