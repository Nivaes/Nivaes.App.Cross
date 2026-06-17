using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

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
                CrossLogHost.Default.Log(LogLevel.Warning, "ViewModel not loaded for view {ViewTypeName}", view.GetType().Name);
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

    [UnconditionalSuppressMessage("Trimming", "IL2072", Justification = "The generic constraint ensures TViewType has the required members")]
    [return: DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)]
    public static Type? FindAssociatedViewModelTypeOrNull<[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors | DynamicallyAccessedMemberTypes.PublicProperties)] TViewType>(
            this TViewType view)
        where TViewType : ICrossView
    {
        ArgumentNullException.ThrowIfNull(view);

        //var associatedTypeFinder = IPlatformApplication.Current!.Services.GetRequiredService<ICrossViewModelTypeFinder>();

        //if (Mvx.IoCProvider?.TryResolve(out ICrossViewModelTypeFinder? associatedTypeFinder) == true)
        //return associatedTypeFinder?.FindTypeOrNull(view.GetType());

        //CrossLogHost.Default?.Log(LogLevel.Trace,
        //    "No view model type finder available - assuming we are looking for a splash screen - returning null");

        return typeof(CrossNullViewModel);
    }
}