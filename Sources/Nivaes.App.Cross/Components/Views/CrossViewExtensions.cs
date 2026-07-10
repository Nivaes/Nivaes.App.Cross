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

    [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)]
    public static Type? FindAssociatedViewModelTypeOrNull<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)] TViewType>(
            this TViewType view)
        where TViewType : ICrossView
    {
        ArgumentNullException.ThrowIfNull(view);

        if (Singleton<CrossViewsViewModelManager>.Instance.TryGetValue(view.GetType(), out var viewModelType))
        {
            return viewModelType;
        }

        //var associatedTypeFinder = IPlatformApplication.Current!.Services.GetRequiredService<ICrossViewModelTypeFinder>();

        //if (Mvx.IoCProvider?.TryResolve(out ICrossViewModelTypeFinder? associatedTypeFinder) == true)
        //return associatedTypeFinder?.FindTypeOrNull(view.GetType());

        //CrossLogHost.Default?.Log(LogLevel.Trace,
        //    "No view model type finder available - assuming we are looking for a splash screen - returning null");

        //return typeof(CrossNullViewModel);
        return null;
    }
}