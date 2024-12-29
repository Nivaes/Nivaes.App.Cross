namespace Nivaes.App.Cross
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public abstract class ViewDispatcher : IViewDispatcher
    {
        protected ViewDispatcher()
        {
        }

        public abstract Task<bool> ShowViewModel(IViewModelRequest request);
    }
}
