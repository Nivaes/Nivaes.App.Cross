using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nivaes.App.Cross.Presenters;

namespace Nivaes.App.Cross.WinUI.Presenters
{
    public class WinUIViewPresenter : IViewPresenter
    {
        public WinUIViewPresenter()
        {
        }

        public Task<bool> Show(IViewModelRequest request)
        {
            return Task.FromResult(false);
        }
    }
}
