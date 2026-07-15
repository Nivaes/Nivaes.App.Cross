using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;
using Nivaes.App.Cross.Observability;

namespace Nivaes.App.Cross;

public static class CrossViewExtensions
{
    extension(ICrossView view)
    {
        public void OnViewCreate(Func<ICrossViewModel?> viewModelLoader)
        {
            // note - we check the DataContent before the ViewModel to avoid casting errors
            //       in the case of 'simple' binding code
            if (view.DataContext != null)
                return;

            if (view.ViewModel != null)
                return;

            var viewModel = viewModelLoader();
            if (viewModel == null)
            {
                CrossLoggerHost.GetLogger(nameof(CrossViewExtensions)).LogError("ViewModel not loaded for view {ViewTypeName}", view.GetType().Name);
                return;
            }

            view.ViewModel = viewModel;
        }

        public void OnViewDestroy()
        {
            // nothing needed currently
        }

        public ICrossBundle CreateSaveStateBundle()
        {
            var viewModel = view.ViewModel;
            return viewModel == null ? new CrossBundle() : viewModel.SaveStateBundle();
        }
    }

    public static Type? FindAssociatedViewModelTypeOrNull<TViewType>(
            this TViewType view)
        where TViewType : ICrossView
    {
        if (Singleton<ViewsViewKeyContainerManager>.Instance.TryGetValue(view.GetType(), out var viewModelType))
        {
            return viewModelType;
        }

        return null;
    }
}