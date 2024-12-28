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

        protected static (Type, IViewPresentation) GetViewPresentation(IViewModelRequest request)
        {
            var viewsManager = Singleton<ViewsManager>.Instance;
            var viewModelType = request.ViewModel.GetType();

            if (viewsManager.TryGetValue(viewModelType, out var view))
            {
                var viewPresentationsManager = Singleton<ViewPresentationsManager>.Instance;

                if (viewPresentationsManager.TryGetValue(view, out var viewPresentationType))
                {
                    var container = Singleton<CrossIoCServiceContainer>.Instance;
                    var viewPresentation = container.Resolve(viewPresentationType) as IViewPresentation;

                    return (viewModelType!, viewPresentation!);
                }

                throw new CrossException($"ViewPresentationsManager not found for {view}");
            }

            throw new CrossException($"View not found for {viewModelType}");
        }
    }
}
