using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Nivaes.App.Cross.Presenters;
using Nivaes.App.Cross.WinUI.Presenters;

namespace Nivaes.App.Cross.WinUI
{
    public class WinUIViewDispatcher : IViewDispatcher
    {
        private readonly IViewPresenter mViewPresenter;

        public WinUIViewDispatcher(IViewPresenter viewPresenter)
        {
            mViewPresenter = viewPresenter;
        }

        public async Task<bool> ShowViewModel(IViewModelRequest request)
        {
            await mViewPresenter.Show(request);

            return true;
        }
    }
}
