using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nivaes.App.Cross.Presenters
{
    public abstract class CrossViewPresenter : ICrossViewPresenter
    {
        protected CrossViewPresenter()
        {
        }

        public abstract Task<bool> Show(ICrossViewModelRequest request);

        public abstract Task<bool> Close(ICrossViewModel request);

        protected static (Type, ICrossViewPresentation) GetViewPresentation(ICrossViewModelRequest request)
        {
            var viewsManager = Singleton<CrossViewsManager>.Instance;
            var viewModelType = request.ViewModel.GetType();

            if (viewsManager.TryGetValue(viewModelType, out var viewType))
            {
                var viewPresentationsManager = Singleton<CrossViewPresentationsManager>.Instance;

                try
                {
                    if (viewPresentationsManager.TryGetValue(viewType, out var viewPresentationType))
                    {
                        var container = Singleton<CrossIoCServiceContainer>.Instance;
                        var viewPresentation = container.Resolve(viewPresentationType) as ICrossViewPresentation;

                        return (viewType!, viewPresentation!);
                    }
                }
                catch (Exception ex)
                {
                    throw new CrossException($"Error creating ViewPresentation for {viewType}", ex);
                }
                
                throw new CrossException($"ViewPresentationsManager not found for {viewType}");
            }

            throw new CrossException($"View not found for {viewModelType}");
        }
    }
}
